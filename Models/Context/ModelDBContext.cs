using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyWebAPI.Models;

namespace MyWebAPI.Models.Context;

public class ModelDBContext : DbContext
{
  public ModelDBContext(DbContextOptions<ModelDBContext> options) : base(options)
  {
  }
  
  public DbSet<User> Users => Set<User>();
  public DbSet<Teacher> Teachers => Set<Teacher>();

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // Fallback configuration so the context can be created without DI wiring.
    if (!optionsBuilder.IsConfigured)
    {
      var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile("appsettings.Development.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

      var connectionString = configuration.GetConnectionString("DefaultConnection");

      if (string.IsNullOrWhiteSpace(connectionString))
      {
        throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
      }

      optionsBuilder.UseNpgsql(connectionString);
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Teacher>(entity =>
    {
      entity.ToTable("teacher_table");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

      entity.HasMany(t => t.Users)
        .WithOne(u => u.Teacher)
        .HasForeignKey(u => u.TeacherId)
        .OnDelete(DeleteBehavior.SetNull);
    });
    
    modelBuilder.Entity<User>(entity =>
    {
      entity.ToTable("user_table");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
      entity.Property(x => x.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
      entity.Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(50);
      entity.Property(x => x.Address).HasColumnName("address").HasMaxLength(100);
      entity.Property(x => x.Gender).HasColumnName("gender");
      entity.Property(x => x.TeacherId).HasColumnName("teacher_id");
    });
    
    base.OnModelCreating(modelBuilder);
  }
}
