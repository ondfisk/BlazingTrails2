namespace BlazingTrails.Api.Integration.Tests;

public class PostTrailEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Invoke()
    {
        var uri = PostTrailEndpoint.RouteTemplate;
        var trail = new TrailDTO
        {
            Name = "Trail 3",
            Description = "...",
            Location = "...",
            TimeInMinutes = 10,
            LengthInKilometers = 0.5,
            Route =
            [
                new()
                {
                    Stage = 1,
                    Description = "Stage 1 Description",
                }
            ]
        };
        var response = await _client.PostAsJsonAsync(uri, trail);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location!.ToString().Should().MatchRegex(GetTrailEndpoint.RouteTemplate.Replace("{trailId}", "[0-9a-f-]+"));

        var id = response.Headers.Location.ToString().Split('/').Last();

        using var scope = factory.Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        var entity = await context.Trails.FindAsync(Guid.Parse(id));
        Assert.NotNull(entity);
        entity.Name.Should().Be("Trail 3");
    }

    [Fact]
    public async Task Invoke_given_invalid_trail_does_not_create()
    {
        var uri = PostTrailEndpoint.RouteTemplate;
        var trail = new TrailDTO
        {
            Name = "Trail 4",
            Description = "...",
            Location = "...",
            TimeInMinutes = 0,
            LengthInKilometers = 0,
            Route =
            [
                new()
                {
                    Stage = 1,
                    Description = "Stage 1 Description",
                }
            ]
        };
        var response = await _client.PostAsJsonAsync(uri, trail);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        using var scope = factory.Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        var entity = await context.Trails.FindAsync(Guid.Parse("ef45a5b6-c6eb-49ae-b4bb-c330fc400d5b"));
        entity.Should().BeNull();
    }
}
