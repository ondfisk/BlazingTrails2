namespace BlazingTrails.Core;

public interface ITrailRepository
{
    Task Create(Trail trail, CancellationToken cancellationToken = default);

    Task<Trail?> Read(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Trail>> Read(CancellationToken cancellationToken = default);

    Task Update(Trail trail, CancellationToken cancellationToken = default);

    Task UpdateImage(TrailImage image, CancellationToken cancellationToken = default);

    Task Delete(Guid id, CancellationToken cancellationToken = default);
}
