namespace FootballSolution.Tests;

using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class DbContextFixture: IDisposable
{
    public ApplicationDbContext Context { get; }

    public DbContextFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
    
}