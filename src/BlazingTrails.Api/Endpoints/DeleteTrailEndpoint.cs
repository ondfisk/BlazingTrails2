namespace BlazingTrails.Api.Endpoints;

public static class DeleteTrailEndpoint
{
    public const string RouteTemplate = "/api/v1/trails/{trailId}";

    public static async Task<NoContent> Invoke(ITrailsRepository repository, Guid trailId, CancellationToken cancellationToken)
    {
        await repository.Delete(trailId, cancellationToken);

        return TypedResults.NoContent();
    }
}
