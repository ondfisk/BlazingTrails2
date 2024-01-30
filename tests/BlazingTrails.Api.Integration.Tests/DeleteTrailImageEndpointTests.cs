namespace BlazingTrails.Api.Integration.Tests;

public class DeleteTrailImageEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Invoke()
    {
        var uri = DeleteTrailImageEndpoint.RouteTemplate.Replace("{trailId}", "06cc3033-8c20-4c86-b33a-b3645cf86d91");
        var response = await _client.DeleteAsync(uri);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = factory.Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        var trail = await context.Trails.FindAsync(Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91"));
        Assert.NotNull(trail);
        trail.ImageUrl.Should().BeNull();
    }
}
