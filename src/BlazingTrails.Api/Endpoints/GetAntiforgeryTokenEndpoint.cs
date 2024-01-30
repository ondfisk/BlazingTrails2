namespace BlazingTrails.Api.Endpoints;

public static class GetAntiforgeryTokenEndpoint
{
    public const string RouteTemplate = "/api/v1/antiforgery/token";

    public static ContentHttpResult Invoke(IAntiforgery antiforgery, HttpContext context)
    {
        var tokens = antiforgery.GetAndStoreTokens(context);

        return TypedResults.Content(tokens.RequestToken, "text/plain");
    }
}
