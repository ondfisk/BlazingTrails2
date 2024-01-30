namespace BlazingTrails.Api.Integration.Tests;

public class GetTrailsEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Invoke()
    {
        var uri = GetTrailsEndpoint.RouteTemplate;
        var trails = await _client.GetFromJsonAsync<IReadOnlyList<Trail>>(uri);

        Assert.NotNull(trails);
        trails.Count.Should().Be(2);
    }
}
