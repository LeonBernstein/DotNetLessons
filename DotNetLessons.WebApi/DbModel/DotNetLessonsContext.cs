using DotNetLessons.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetLessons.WebApi.DbModel;

public class DotNetLessonsContext : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;
    public DbSet<Address> Addresses { get; set; } = null!;

    public DotNetLessonsContext(
          DbContextOptions<DotNetLessonsContext> options
    ) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entityBuilder =>
        {
            entityBuilder.HasKey(e => e.PersonId);

            entityBuilder.Property(e => e.PersonId)
                .UseIdentityColumn();

            entityBuilder.HasMany(e => e.AddressesNavigations)
                .WithOne(f => f.PersonNavigation)
                .HasForeignKey(f => f.PersonId);
        });

        modelBuilder.Entity<Address>(entityBuilder =>
        {
            entityBuilder.HasKey(e => e.AddressId);

            entityBuilder.Property(e => e.AddressId)
                .UseIdentityColumn();
        });
    }
}
