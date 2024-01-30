namespace BlazingTrails.Api.Endpoints;

public static class DeleteTrailEndpoint
{
    public const string RouteTemplate = "/api/v1/trails/{trailId}";

    public static async Task<NoContent> Invoke(Guid trailId, ITrailRepository repository, CancellationToken cancellationToken = default)
    {
        await repository.Delete(trailId, cancellationToken);

        return TypedResults.NoContent();
    }
}
