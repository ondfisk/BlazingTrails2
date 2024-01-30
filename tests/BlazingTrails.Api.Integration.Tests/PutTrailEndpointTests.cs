namespace BlazingTrails.Api.Integration.Tests;

public class PutTrailEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Invoke()
    {
        var uri = PutTrailEndpoint.RouteTemplate.Replace("{trailId}", "06cc3033-8c20-4c86-b33a-b3645cf86d91");
        var trail = new TrailDTO
        {
            Name = "Trail 2 Updated",
            Description = "...",
            Location = "...",
            TimeInMinutes = 10,
            LengthInKilometers = 0.5,
            Route =
            [
                new()
                {
                    Stage = 1,
                    Description = "Stage 1 Description Updated",
                }
            ]
        };
        var response = await _client.PutAsJsonAsync(uri, trail);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        using var scope = factory.Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        var entity = await context.Trails.FindAsync(Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91"));
        Assert.NotNull(entity);
        entity.Name.Should().Be("Trail 2 Updated");
    }

    [Fact]
    public async Task Invoke_given_invalid_trail_does_not_update()
    {
        var uri = PutTrailEndpoint.RouteTemplate.Replace("{trailId}", "06cc3033-8c20-4c86-b33a-b3645cf86d91");
        var trail = new TrailDTO
        {
            Name = "Trail 2 Updated",
            Description = "...",
            Location = "...",
            TimeInMinutes = 0,
            LengthInKilometers = 0,
            Route =
            [
                new()
                {
                    Stage = 1,
                    Description = "Stage 1 Description Updated",
                }
            ]
        };
        var response = await _client.PutAsJsonAsync(uri, trail);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        using var scope = factory.Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        var entity = await context.Trails.FindAsync(Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91"));
        Assert.NotNull(entity);
        entity.Name.Should().Be("Trail 2");
    }
}
