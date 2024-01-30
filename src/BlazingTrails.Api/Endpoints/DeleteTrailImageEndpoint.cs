namespace BlazingTrails.Api.Endpoints;

public static class DeleteTrailImageEndpoint
{
    public const string RouteTemplate = "/api/v1/trails/{trailId}/image";

    public static async Task<NoContent> Invoke(Guid trailId, ITrailRepository repository, CancellationToken cancellationToken = default)
    {
        await repository.UpdateImage(new TrailImage(trailId, default), cancellationToken);

        return TypedResults.NoContent();
    }
}
