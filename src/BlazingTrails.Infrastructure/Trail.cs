namespace BlazingTrails.Infrastructure;

public class TrailEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Image { get; set; }
    public required string Location { get; set; }
    public int TimeInMinutes { get; set; }
    public double LengthInKilometers { get; set; }
    public ICollection<RouteInstructionEntity> Route { get; set; } = [];
}

public class TrailConfig : IEntityTypeConfiguration<TrailEntity>
{
    public void Configure(EntityTypeBuilder<TrailEntity> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(250);
        builder.Property(x => x.Image).HasMaxLength(100);
        builder.Property(x => x.Location).HasMaxLength(50);
    }
}
