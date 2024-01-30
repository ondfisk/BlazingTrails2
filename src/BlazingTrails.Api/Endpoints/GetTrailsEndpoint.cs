namespace BlazingTrails.Api.Endpoints;

public static class GetTrailsEndpoint
{
    public const string RouteTemplate = "/api/v1/trails";

    public static async Task<Ok<IEnumerable<Trail>>> Invoke(ITrailsRepository repository, CancellationToken cancellationToken)
        => TypedResults.Ok(await repository.Read(cancellationToken).ConfigureAwait(false));
}
