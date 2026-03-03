using System;
using System.Threading.Tasks;
using CI_CD_Test.Domain.Entities;
using CI_CD_Test.Infrastructure.Data;
using CI_CD_Test.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CI_CD_Test.Tests.Repositories;

public class ProductRepositoryTests
{
    private static AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAndGetById_Works()
    {
        await using var context = CreateInMemoryContext();
        var repo = new ProductRepository(context);

        var product = new Product
        {
            Name = "Test Product",
            Price = 10.5m,
            Stock = 5
        };

        var created = await repo.AddAsync(product);

        var fromDb = await repo.GetByIdAsync(created.Id);

        Assert.NotNull(fromDb);
        Assert.Equal("Test Product", fromDb!.Name);
    }

    [Fact]
    public async Task Update_ChangesArePersisted()
    {
        await using var context = CreateInMemoryContext();
        var repo = new ProductRepository(context);

        var product = new Product { Name = "Old", Price = 1, Stock = 1 };
        var created = await repo.AddAsync(product);

        created.Name = "New Name";
        await repo.UpdateAsync(created);

        var fromDb = await repo.GetByIdAsync(created.Id);

        Assert.Equal("New Name", fromDb!.Name);
    }

    [Fact]
    public async Task Delete_RemovesEntity()
    {
        await using var context = CreateInMemoryContext();
        var repo = new ProductRepository(context);

        var product = new Product { Name = "ToDelete", Price = 1, Stock = 1 };
        var created = await repo.AddAsync(product);

        await repo.DeleteAsync(created.Id);

        var fromDb = await repo.GetByIdAsync(created.Id);

        Assert.Null(fromDb);
    }
}

