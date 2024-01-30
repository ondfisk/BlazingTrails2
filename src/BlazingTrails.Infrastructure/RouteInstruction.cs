namespace BlazingTrails.Infrastructure;

public sealed class RouteInstructionEntity
{
    public Guid TrailId { get; set; }
    public int Stage { get; set; }
    public required string Description { get; set; }
    public TrailEntity Trail { get; set; } = default!;
}

public sealed class RouteInstructionConfig : IEntityTypeConfiguration<RouteInstructionEntity>
{
    public void Configure(EntityTypeBuilder<RouteInstructionEntity> builder)
    {
        builder.HasKey(x => new { x.TrailId, x.Stage });
        builder.Property(x => x.Description).HasMaxLength(250);
    }
}