using Microsoft.EntityFrameworkCore;
using SGEI.Models;

namespace SGEI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
    public DbSet<Anamnese> Anamneses { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Procedimento> Procedimentos { get; set; }
    public DbSet<ConfiguracaoClinica> ConfiguracoesClinica { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Procedimento>()
            .Property(p => p.Preco)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.ValorPago)
            .HasColumnType("decimal(18,2)");
    }
}