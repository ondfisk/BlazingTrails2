namespace BlazingTrails.Api.Integration.Tests;

public class PutTrailImageEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Invoke()
    {
        var uri = PutTrailImageEndpoint.RouteTemplate.Replace("{trailId}", "06cc3033-8c20-4c86-b33a-b3645cf86d91");

        using var formData = new MultipartFormDataContent();
        formData.Headers.Add("X-XSRF-TOKEN", await _client.GetStringAsync("/api/v1/antiforgery/token"));
        var stream = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Images", "trail.jpg"));
        var content = new StreamContent(stream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        formData.Add(content, "file", "trail.jpg");

        var response = await _client.PutAsync(uri, formData);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var expectedImageUrl = new Uri(_client.BaseAddress!, "Images/06cc3033-8c20-4c86-b33a-b3645cf86d91.jpg").ToString();
        response.Headers.Location.Should().Be(expectedImageUrl);

        using var scope = factory.Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        var entity = await context.Trails.FindAsync(Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91"));
        Assert.NotNull(entity);
        entity.ImageUrl.Should().Be(expectedImageUrl);

        var image = await _client.GetAsync(entity.ImageUrl);
        image.Content.Headers.ContentType!.MediaType.Should().Be("image/jpeg");

        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", "06cc3033-8c20-4c86-b33a-b3645cf86d91.jpg"));
    }

    [Fact]
    public async Task Invoke_given_invalid_image_does_not_update()
    {
        var uri = PutTrailImageEndpoint.RouteTemplate.Replace("{trailId}", "06cc3033-8c20-4c86-b33a-b3645cf86d91");

        using var formData = new MultipartFormDataContent();
        formData.Headers.Add("X-XSRF-TOKEN", await _client.GetStringAsync("/api/v1/antiforgery/token"));
        var stream = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Images", "invalid.txt"));
        var content = new StreamContent(stream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        formData.Add(content, "file", "invalid.txt");

        var response = await _client.PutAsync(uri, formData);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
