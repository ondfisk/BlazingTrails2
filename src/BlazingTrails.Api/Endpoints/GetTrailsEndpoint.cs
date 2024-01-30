namespace BlazingTrails.Api.Endpoints;

public static class GetTrailsEndpoint
{
    public const string RouteTemplate = "/api/v1/trails";

    public static async Task<Ok<IReadOnlyList<Trail>>> Invoke(ITrailRepository repository, CancellationToken cancellationToken = default)
        => TypedResults.Ok(await repository.Read(cancellationToken).ConfigureAwait(false));
}
