namespace BlazingTrails.Core;

public interface ITrailsRepository
{
    Task Create(Trail trail, CancellationToken cancellationToken);

    Task<Trail?> Read(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<Trail>> Read(CancellationToken cancellationToken);

    Task Update(Trail trail, CancellationToken cancellationToken);

    Task UpdateTrailImage(TrailImage image, CancellationToken cancellationToken);

    Task Delete(Guid id, CancellationToken cancellationToken);
}
