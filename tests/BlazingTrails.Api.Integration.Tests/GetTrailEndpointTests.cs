namespace BlazingTrails.Api.Integration.Tests;

public class GetTrailEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Invoke()
    {
        var uri = GetTrailEndpoint.RouteTemplate.Replace("{trailId}", "06cc3033-8c20-4c86-b33a-b3645cf86d91");
        var trail = await _client.GetFromJsonAsync<Trail>(uri);

        Assert.NotNull(trail);
        trail.Name.Should().Be("Trail 2");
    }
}
