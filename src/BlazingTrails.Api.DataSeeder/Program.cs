using System.Net.Http.Json;
using System.Text.Json.Nodes;
using BlazingTrails.Core;

var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data/trails/trail-data.json"));
dynamic trails = JsonNode.Parse(json)!;

var client = new HttpClient { BaseAddress = new Uri($"http://localhost:5062") };

foreach (dynamic trail in trails)
{
    var trailDTO = new TrailDTO
    {
        Name = (string)trail["name"],
        Description = (string)trail["description"],
        Location = (string)trail["location"],
        LengthInKilometers = (double)trail["lengthInKilometers"],
        TimeInMinutes = (int)trail["timeInMinutes"]
    };
    foreach (var instruction in trail["route"])
    {
        trailDTO.Route.Add(new() { Stage = (int)instruction["stage"], Description = (string)instruction["description"] });
    }
    var image = (string)trail["image"];
    var response = await client.PostAsJsonAsync("api/v1/trails", trailDTO);
    var created = await response.Content.ReadFromJsonAsync<Trail>();
    Console.WriteLine($"Created trail {created!.Id} - {created.Name}");

    using var formData = new MultipartFormDataContent();
    formData.Headers.Add("X-XSRF-TOKEN", await client.GetStringAsync("/api/v1/antiforgery/token"));
    var stream = File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data/trails", image));
    var content = new StreamContent(stream);
    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
    formData.Add(content, "file", image);

    var response2 = await client.PutAsync($"api/v1/trails/{created.Id}/image", formData);
    Console.WriteLine($"- Updated trail image {response2.Headers.Location}");
    Console.WriteLine();
}