using beestje_op_je_feestje.Models;
using Microsoft.EntityFrameworkCore;

public class AnimalPartyContext : DbContext
{
    public DbSet<Animal> animals { get; set; }

    public AnimalPartyContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>()
            .HasKey(a => a._id);  
    }


}