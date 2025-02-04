using beestje_op_je_feestje.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AnimalPartyContext : IdentityDbContext
{
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public AnimalPartyContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Booking) 
            .WithMany(b => b.Animals) 
            .HasForeignKey(a => a.BookingId) 
            .OnDelete(DeleteBehavior.SetNull); 

        modelBuilder.Entity<Account>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Booking>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Booking>()
            .HasMany(b => b.Animals)
            .WithOne(a => a.Booking)
            .HasForeignKey(a => a.BookingId);
    }


}