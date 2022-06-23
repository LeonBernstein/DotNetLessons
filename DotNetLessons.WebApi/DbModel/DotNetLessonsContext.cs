using DotNetLessons.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetLessons.WebApi.DbModel;

public class DotNetLessonsContext : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;
    public DbSet<Address> Addresses { get; set; } = null!;


    public string DbPath { get; }

    public DotNetLessonsContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "DotNetLessons.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entityBuilder =>
        {
            entityBuilder.HasKey(e => e.PersonId);

            entityBuilder.HasMany(e => e.AddressesNavigations)
                .WithOne(f => f.PersonNavigation)
                .HasForeignKey(f => f.PersonId);
        });

        modelBuilder.Entity<Address>(entityBuilder =>
        {
            entityBuilder.HasKey(e => e.AddressId);
        });
    }
}
