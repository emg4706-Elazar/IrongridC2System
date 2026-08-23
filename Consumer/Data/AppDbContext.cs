using Microsoft.EntityFrameworkCore;
using Consumer.Models;

namespace Consumer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AssetLiveStatus> AssetLiveStatus => Set<AssetLiveStatus>();
}
