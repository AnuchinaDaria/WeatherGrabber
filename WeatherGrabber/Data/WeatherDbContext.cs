using Microsoft.EntityFrameworkCore;
using WeatherGrabber.Models;

namespace WeatherGrabber.Data;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
    {
    }

    public DbSet<WeatherHistory> WeatherHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // предотвращаем дубликаты записей для одного города и времени
        modelBuilder.Entity<WeatherHistory>()
            .HasIndex(w => new { w.City, w.TimestampUtc })
            .IsUnique();
    }
} 