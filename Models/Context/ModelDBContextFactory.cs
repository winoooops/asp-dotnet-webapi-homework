using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyWebAPI.Models.Context;

public class ModelDBContextFactory : IDesignTimeDbContextFactory<ModelDBContext>
{
  public ModelDBContext CreateDbContext(string[] args)
  {
    var configuration = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", false)
      .AddJsonFile("appsettings.Development.json", true)
      .AddEnvironmentVariables()
      .Build();

    var connectionStr = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string not found");

    var optionBuilder = new DbContextOptionsBuilder<ModelDBContext>();

    optionBuilder.UseNpgsql(connectionStr);
    
    return new ModelDBContext(optionBuilder.Options);
  }
}