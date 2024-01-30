namespace BlazingTrails.Api.Integration.Tests;

public class DeleteTrailEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Invoke()
    {
        var uri = DeleteTrailEndpoint.RouteTemplate.Replace("{trailId}", "06cc3033-8c20-4c86-b33a-b3645cf86d91");
        var response = await _client.DeleteAsync(uri);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = factory.Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        var count = await context.Trails.CountAsync();
        count.Should().Be(1);
        var entity = await context.Trails.FindAsync(Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91"));
        entity.Should().BeNull();
    }
}
