namespace BlazingTrails.Api.Endpoints;

public static class GetTrailEndpoint
{
    public const string RouteTemplate = "/api/v1/trails/{trailId}";

    public static async Task<Results<Ok<Trail>, NotFound>> Invoke(ITrailsRepository repository, Guid trailId, CancellationToken cancellationToken)
    {
        var trail = await repository.Read(trailId, cancellationToken);

        if (trail is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(trail);
    }
}
