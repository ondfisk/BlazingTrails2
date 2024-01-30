using BlazingTrails.Infrastructure;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.AddDbContext<BlazingTrailsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(BlazingTrailsContext)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
    RequestPath = new PathString("/Images")
});

app.MapGet("/api/v1/antiforgery/token", (IAntiforgery antiforgery, HttpContext context) =>
{
    var tokens = antiforgery.GetAndStoreTokens(context);
    return TypedResults.Content(tokens.RequestToken, "text/plain");
});

app.MapGet(GetTrailEndpoint.RouteTemplate, GetTrailEndpoint.Invoke);
app.MapGet(GetTrailsEndpoint.RouteTemplate, GetTrailsEndpoint.Invoke);
app.MapPost(PostTrailEndpoint.RouteTemplate, PostTrailEndpoint.Invoke);
app.MapPut(PutTrailEndpoint.RouteTemplate, PutTrailEndpoint.Invoke);
app.MapDelete(DeleteTrailEndpoint.RouteTemplate, DeleteTrailEndpoint.Invoke);
app.MapPost(PutTrailImageEndpoint.RouteTemplate, PutTrailImageEndpoint.Invoke);

app.Run();
