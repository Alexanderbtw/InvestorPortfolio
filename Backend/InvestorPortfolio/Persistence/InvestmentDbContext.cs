using Core.Entities;
using Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class InvestmentDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<BrokerageAccount> BrokerageAccounts { get; set; }
    public DbSet<StockContainer<Share>> Shares { get; set; }
    public DbSet<StockContainer<Bond>> Bonds { get; set; }
    public DbSet<StockContainer<Currency>> Currencies { get; set; }
    public InvestmentDbContext(DbContextOptions<InvestmentDbContext> options) : base(options)
    {
        if (Database.IsRelational()) Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.ToTable("Users");
            user.Property(u => u.Version).IsRowVersion().HasColumnName("Version");
            user.HasIndex(u=> u.Email).IsUnique();
        });

        modelBuilder.Entity<Portfolio>(portfolio =>
        {
            portfolio.ToTable("Users");
            portfolio
                .HasOne(p => p.Owner).WithOne()
                .HasForeignKey<User>(p => p.Id);
            portfolio.Navigation(p => p.Owner).IsRequired();
            portfolio.Property<byte[]>("Version").IsRowVersion().HasColumnName("Version");
        });

        modelBuilder.Entity<BrokerageAccount>(brokerageAccount =>
        {
            brokerageAccount.HasIndex(a => a.Title).IsUnique();
        });
    }
}