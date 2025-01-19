using beestje_op_je_feestje.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AnimalPartyContext : IdentityDbContext
{
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Account> Accounts { get; set; }

    public AnimalPartyContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Animal>()
            .HasKey(a => a.Id);
    }


}