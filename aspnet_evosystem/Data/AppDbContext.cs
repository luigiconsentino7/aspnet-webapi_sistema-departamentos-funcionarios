using aspnet_evosystem.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnet_evosystem.Data
{

    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Funcionario> FuncionariosDb { get; set; }
        public DbSet<Departamento> DepartamentosDb { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Departamento)
                .WithMany(d => d.Funcionarios)
                .HasForeignKey(f => f.DepartamentoId);

            modelBuilder.Entity<Departamento>()
                .HasMany(d => d.Funcionarios);
        }

    }

 }

