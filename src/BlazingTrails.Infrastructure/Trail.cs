namespace BlazingTrails.Infrastructure;

public class TrailEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? ImageUrl { get; set; }
    public required string Location { get; set; }
    public int TimeInMinutes { get; set; }
    public double LengthInKilometers { get; set; }
    public IList<RouteInstructionEntity> Route { get; set; } = [];
}

public sealed class TrailConfig : IEntityTypeConfiguration<TrailEntity>
{
    public void Configure(EntityTypeBuilder<TrailEntity> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.ImageUrl).HasMaxLength(100);
        builder.Property(x => x.Location).HasMaxLength(50);
    }
}
