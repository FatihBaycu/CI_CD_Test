using CI_CD_Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CI_CD_Test.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}

