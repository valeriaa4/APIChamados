using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using static APIChamados.Data.ApplicationDBContext;
using LicenseType = QuestPDF.Infrastructure.LicenseType;

namespace APIChamados.Controllers
{
    [ApiController]
    [Route("api/relatorio")]
    public class RelatorioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/relatorio/excel
        [HttpGet("excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var dados = await _context.Chamados
                .AsNoTracking()
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .OrderBy(c => c.IdChamado)
                .ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Chamados");

            // Cabeçalho
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Protocolo";
            ws.Cell(1, 3).Value = "Título";
            ws.Cell(1, 4).Value = "Descrição";
            ws.Cell(1, 5).Value = "Data Abertura";
            ws.Cell(1, 6).Value = "Data Conclusão";
            ws.Cell(1, 7).Value = "Status";
            ws.Cell(1, 8).Value = "Prioridade";
            ws.Cell(1, 9).Value = "Usuário";
            ws.Cell(1, 10).Value = "Técnico";
            ws.Cell(1, 11).Value = "Solução";
            ws.Cell(1, 12).Value = "Interações";

            var header = ws.Range(1, 1, 1, 12);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.LightGray;

            var row = 2;
            foreach (var c in dados)
            {
                ws.Cell(row, 1).Value = c.IdChamado;
                ws.Cell(row, 2).Value = c.Protocolo;
                ws.Cell(row, 3).Value = c.Titulo;
                ws.Cell(row, 4).Value = c.Descricao;
                ws.Cell(row, 5).Value = c.DataAbertura.ToString("g", CultureInfo.GetCultureInfo("pt-BR"));
                ws.Cell(row, 6).Value = c.DataConclusao?.ToString("g", CultureInfo.GetCultureInfo("pt-BR")) ?? "";
                ws.Cell(row, 7).Value = c.Status.ToString();
                ws.Cell(row, 8).Value = c.Prioridade.ToString();
                ws.Cell(row, 9).Value = c.Usuario?.Nome ?? "";
                ws.Cell(row, 10).Value = c.Tecnico?.Nome ?? "";
                ws.Cell(row, 11).Value = c.Solucao?.Descricao ?? "";
                ws.Cell(row, 12).Value = c.HistoricoInteracoes?.Count ?? 0;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            var bytes = ms.ToArray();

            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(bytes, contentType, "chamados.xlsx");
        }

        // GET: api/relatorio/pdf
        [HttpGet("pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var dados = await _context.Chamados
                .AsNoTracking()
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Solucao)
                .Include(c => c.HistoricoInteracoes)
                .OrderBy(c => c.IdChamado)
                .ToListAsync();

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(24);
                    page.Header().Text("Relatório de Chamados").SemiBold().FontSize(16).AlignCenter();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(1); // ID
                            cols.RelativeColumn(2); // Protocolo
                            cols.RelativeColumn(4); // Título
                            cols.RelativeColumn(6); // Descrição
                            cols.RelativeColumn(3); // Data Abertura
                            cols.RelativeColumn(3); // Status/Prioridade
                            cols.RelativeColumn(3); // Usuário/Técnico
                            cols.RelativeColumn(2); // Interações
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellHeader).Text("ID");
                            header.Cell().Element(CellHeader).Text("Protocolo");
                            header.Cell().Element(CellHeader).Text("Título");
                            header.Cell().Element(CellHeader).Text("Descrição");
                            header.Cell().Element(CellHeader).Text("Data Abertura");
                            header.Cell().Element(CellHeader).Text("Status / Prioridade");
                            header.Cell().Element(CellHeader).Text("Usuário / Técnico");
                            header.Cell().Element(CellHeader).Text("Interações");
                        });

                        foreach (var c in dados)
                        {
                            table.Cell().Element(CellBody).Text(c.IdChamado.ToString());
                            table.Cell().Element(CellBody).Text(c.Protocolo.ToString());
                            table.Cell().Element(CellBody).Text(c.Titulo ?? "");
                            table.Cell().Element(CellBody).Text(c.Descricao ?? "");
                            table.Cell().Element(CellBody).Text(c.DataAbertura.ToString("g", CultureInfo.GetCultureInfo("pt-BR")));
                            table.Cell().Element(CellBody).Text($"{c.Status} / {c.Prioridade}");
                            table.Cell().Element(CellBody).Text($"{c.Usuario?.Nome ?? ""} / {c.Tecnico?.Nome ?? ""}");
                            table.Cell().Element(CellBody).Text((c.HistoricoInteracoes?.Count ?? 0).ToString());
                        }

                        IContainer CellHeader(IContainer c) => c
                            .DefaultTextStyle(x => x.SemiBold())
                            .Padding(4)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Medium);

                        IContainer CellBody(IContainer c) => c.Padding(4);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Gerado em ").Light();
                        txt.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    });
                });
            });

            var pdfBytes = doc.GeneratePdf();
            return File(pdfBytes, "application/pdf", "chamados.pdf");
        }
    }
}
