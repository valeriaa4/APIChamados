using APIChamados.Models;
using Microsoft.EntityFrameworkCore;

namespace APIChamados.Data
{
    public class ApplicationDBContext
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Chamado> Chamados { get; set; }
            public DbSet<Usuario> Usuarios { get; set; }
            public DbSet<Tecnico> Tecnicos { get; set; }
            public DbSet<Solucao> Solucoes { get; set; }
            public DbSet<Interacao> Interacoes { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
 
                modelBuilder.Entity<Usuario>()
                    .HasIndex(u => u.Email)
                    .IsUnique();

                modelBuilder.Entity<Chamado>()
                    .HasOne(c => c.Usuario)
                    .WithMany()
                    .HasForeignKey(c => c.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);

                // Chamado -> Usuario (técnico atribuído) : many-to-one
                modelBuilder.Entity<Chamado>()
                    .HasOne(c => c.Tecnico)
                    .WithMany() // Tecnico é um subtipo de Usuario; não usar coleção inversa aqui
                    .HasForeignKey(c => c.IdTecnico)
                    .OnDelete(DeleteBehavior.Restrict);

                // Chamado -> Interacoes : one-to-many
                modelBuilder.Entity<Interacao>()
                    .HasOne(i => i.Chamado)
                    .WithMany(c => c.HistoricoInteracoes)
                    .HasForeignKey(i => i.IdChamado)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relação 1:1 Chamado <-> Solucao
                modelBuilder.Entity<Chamado>()
                    .HasOne(c => c.Solucao)
                    .WithOne(s => s.Chamado)
                    .HasForeignKey<Solucao>(s => s.IdChamado)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}
