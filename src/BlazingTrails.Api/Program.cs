var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.AddDbContext<BlazingTrailsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(BlazingTrailsContext)));
});
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly, typeof(TrailDTO).Assembly]);

var app = builder.Build();

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
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = new PathString("/Images")
});

app.MapGet(GetAntiforgeryTokenEndpoint.RouteTemplate, GetAntiforgeryTokenEndpoint.Invoke);
app.MapGet(GetTrailEndpoint.RouteTemplate, GetTrailEndpoint.Invoke);
app.MapGet(GetTrailsEndpoint.RouteTemplate, GetTrailsEndpoint.Invoke);
app.MapPost(PostTrailEndpoint.RouteTemplate, PostTrailEndpoint.Invoke);
app.MapPut(PutTrailEndpoint.RouteTemplate, PutTrailEndpoint.Invoke);
app.MapDelete(DeleteTrailEndpoint.RouteTemplate, DeleteTrailEndpoint.Invoke);
app.MapPut(PutTrailImageEndpoint.RouteTemplate, PutTrailImageEndpoint.Invoke);
app.MapDelete(DeleteTrailImageEndpoint.RouteTemplate, DeleteTrailImageEndpoint.Invoke);

app.Run();

public partial class Program { }
