namespace BlazingTrails.Api.Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _mssqlContainer = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlazingTrailsContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BlazingTrailsContext>(options =>
            {
                options.UseSqlServer(_mssqlContainer.GetConnectionString());
            });
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        await _mssqlContainer.StartAsync();
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<BlazingTrailsContext>();
        await context.Database.MigrateAsync();

        var trail1 = new TrailEntity
        {
            Id = Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"),
            Name = "Trail 1",
            Description = "Description for Trail 1",
            ImageUrl = "http://localhost/trail-1.jpg",
            Location = "Location for Trail 1",
            TimeInMinutes = 60,
            LengthInKilometers = 3.5,
            Route =
            [
                new()
                {
                    Stage = 1,
                    Description = "Trail 1: Stage 1"
                },
                new()
                {
                    Stage = 2,
                    Description = "Trail 1: Stage 2"
                }
            ]
        };

        var trail2 = new TrailEntity
        {
            Id = Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91"),
            Name = "Trail 2",
            Description = "Description for Trail 2",
            ImageUrl = "http://localhost/trail-2.jpg",
            Location = "Location for Trail 2",
            TimeInMinutes = 120,
            LengthInKilometers = 9.0,
            Route =
            [
                new()
                {
                    Stage = 1,
                    Description = "Trail 2: Stage 1"
                },
                new()
                {
                    Stage = 2,
                    Description = "Trail 2: Stage 2"
                }
            ]
        };

        context.Trails.AddRange(trail1, trail2);
        await context.SaveChangesAsync();
    }

    Task IAsyncLifetime.DisposeAsync() => _mssqlContainer.DisposeAsync().AsTask();
}