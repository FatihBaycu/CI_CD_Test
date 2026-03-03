using CI_CD_Test.Domain.Entities;
using CI_CD_Test.Infrastructure.Data;
using CI_CD_Test.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CI_CD_Test.Infrastructure.Postgres;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Products.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _dbContext.Products.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

