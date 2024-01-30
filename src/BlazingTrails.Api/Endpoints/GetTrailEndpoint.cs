namespace BlazingTrails.Api.Endpoints;

public static class GetTrailEndpoint
{
    public const string RouteTemplate = "/api/v1/trails/{trailId}";

    public static async Task<Results<Ok<Trail>, NotFound>> Invoke(Guid trailId, ITrailRepository repository, CancellationToken cancellationToken = default)
    {
        var trail = await repository.Read(trailId, cancellationToken);

        if (trail is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(trail);
    }
}
