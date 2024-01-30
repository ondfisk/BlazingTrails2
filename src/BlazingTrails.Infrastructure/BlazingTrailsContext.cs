namespace BlazingTrails.Infrastructure;

public sealed class BlazingTrailsContext(DbContextOptions<BlazingTrailsContext> options) : DbContext(options)
{
    public DbSet<TrailEntity> Trails => Set<TrailEntity>();
    public DbSet<RouteInstructionEntity> RouteInstructions => Set<RouteInstructionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TrailConfig());
        modelBuilder.ApplyConfiguration(new RouteInstructionConfig());
    }
}