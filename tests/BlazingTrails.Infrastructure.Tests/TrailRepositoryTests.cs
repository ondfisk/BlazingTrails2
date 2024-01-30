namespace BlazingTrails.Infrastructure.Tests;

public sealed class TrailRepositoryTests : IAsyncLifetime
{
    private readonly SqliteConnection _connection;
    private readonly BlazingTrailsContext _context;
    private readonly TrailRepository _repository;

    public TrailRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<BlazingTrailsContext>().UseSqlite(_connection);
        _context = new BlazingTrailsContext(builder.Options);
        _repository = new TrailRepository(_context);
    }

    [Fact]
    public async Task Create_creates_trail()
    {
        // Arrange
        var trail3 = new Trail(Guid.Parse("fa90535f-061c-4f8d-9277-fc5d0fabaa0c"), "Trail 3", "Description for Trail 3", "Location for Trail 3", 180, 11.0, "http://localhost/trail-3.jpg",
        [
            new(1, "Trail 3: Stage 1"),
            new(2, "Trail 3: Stage 2")
        ]);

        // Act
        await _repository.Create(trail3);

        // Assert
        var created = await _context.Trails.Include(t => t.Route).FirstOrDefaultAsync(t => t.Id == Guid.Parse("fa90535f-061c-4f8d-9277-fc5d0fabaa0c"));
        Assert.NotNull(created);
        created.Name.Should().Be("Trail 3");
        created.Description.Should().Be("Description for Trail 3");
        created.ImageUrl.Should().BeNull(); // Not set
        created.Location.Should().Be("Location for Trail 3");
        created.TimeInMinutes.Should().Be(180);
        created.LengthInKilometers.Should().Be(11.0);
        created.Route.Should().HaveCount(2);
        created.Route[0].Stage.Should().Be(1);
        created.Route[0].Description.Should().Be("Trail 3: Stage 1");
        created.Route[1].Stage.Should().Be(2);
        created.Route[1].Description.Should().Be("Trail 3: Stage 2");
    }

    [Fact]
    public async Task Read_given_existing_id_returns_trail()
    {
        // Arrange
        var id = Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91");

        // Act
        var trail = await _repository.Read(id);

        // Assert
        Assert.NotNull(trail);
        trail.Id.Should().Be(id);
        trail.Name.Should().Be("Trail 2");
        trail.Description.Should().Be("Description for Trail 2");
        trail.ImageUrl.Should().Be("http://localhost/trail-2.jpg");
        trail.Location.Should().Be("Location for Trail 2");
        trail.TimeInMinutes.Should().Be(120);
        trail.LengthInKilometers.Should().Be(9.0);
        trail.Route.Should().HaveCount(2);
        trail.Route[0].Stage.Should().Be(1);
        trail.Route[0].Description.Should().Be("Trail 2: Stage 1");
        trail.Route[1].Stage.Should().Be(2);
        trail.Route[1].Description.Should().Be("Trail 2: Stage 2");
    }

    [Fact]
    public async Task Read_returns_all_trails_ordered_by_Name()
    {
        // Arrange

        // Act
        var trails = await _repository.Read();

        // Assert
        trails.Count.Should().Be(2);

        var trail1 = trails[0];
        trail1.Id.Should().Be(Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"));
        trail1.Name.Should().Be("Trail 1");
        trail1.Description.Should().Be("Description for Trail 1");
        trail1.ImageUrl.Should().Be("http://localhost/trail-1.jpg");
        trail1.Location.Should().Be("Location for Trail 1");
        trail1.TimeInMinutes.Should().Be(60);
        trail1.LengthInKilometers.Should().Be(3.5);
        trail1.Route.Should().HaveCount(2);
        trail1.Route[0].Stage.Should().Be(1);
        trail1.Route[0].Description.Should().Be("Trail 1: Stage 1");
        trail1.Route[1].Stage.Should().Be(2);
        trail1.Route[1].Description.Should().Be("Trail 1: Stage 2");

        var trail2 = trails[1];
        trail2.Id.Should().Be(Guid.Parse("06cc3033-8c20-4c86-b33a-b3645cf86d91"));
        trail2.Name.Should().Be("Trail 2");
        trail2.Description.Should().Be("Description for Trail 2");
        trail2.ImageUrl.Should().Be("http://localhost/trail-2.jpg");
        trail2.Location.Should().Be("Location for Trail 2");
        trail2.TimeInMinutes.Should().Be(120);
        trail2.LengthInKilometers.Should().Be(9.0);
        trail2.Route.Should().HaveCount(2);
        trail2.Route[0].Stage.Should().Be(1);
        trail2.Route[0].Description.Should().Be("Trail 2: Stage 1");
        trail2.Route[1].Stage.Should().Be(2);
        trail2.Route[1].Description.Should().Be("Trail 2: Stage 2");
    }

    [Fact]
    public async Task Read_given_non_existing_id_returns_null()
    {
        // Arrange
        var id = Guid.Parse("c3b6927c-d294-4799-8865-31c5bf8bdc9b");

        // Act
        var trail = await _repository.Read(id);

        // Assert
        trail.Should().BeNull();
    }

    [Fact]
    public async Task Update_updates_trail_and_adds_stage()
    {
        // Arrange
        var trail4 = new Trail(Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"), "Trail 4", "Description for Trail 4", "Location for Trail 4", 90, 6.5, "http://localhost/trail-4.jpg",
        [
            new(3, "Trail 4: Stage 1"),
            new(8, "Trail 4: Stage 2"),
            new(10, "Trail 4: Stage 3")
        ]);

        // Act
        await _repository.Update(trail4);

        // Assert
        var updated = await _context.Trails.Include(t => t.Route).FirstOrDefaultAsync(t => t.Id == Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"));
        Assert.NotNull(updated);
        updated.Name.Should().Be("Trail 4");
        updated.Description.Should().Be("Description for Trail 4");
        updated.ImageUrl.Should().Be("http://localhost/trail-1.jpg"); // Unchanged
        updated.Location.Should().Be("Location for Trail 4");
        updated.TimeInMinutes.Should().Be(90);
        updated.LengthInKilometers.Should().Be(6.5);
        updated.Route.Should().HaveCount(3);
        updated.Route[0].Stage.Should().Be(1);
        updated.Route[0].Description.Should().Be("Trail 4: Stage 1");
        updated.Route[1].Stage.Should().Be(2);
        updated.Route[1].Description.Should().Be("Trail 4: Stage 2");
        updated.Route[2].Stage.Should().Be(3);
        updated.Route[2].Description.Should().Be("Trail 4: Stage 3");
    }

    [Fact]
    public async Task Update_updates_trail_and_removes_stage()
    {
        // Arrange
        var trail4 = new Trail(Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"), "Trail 4", "Description for Trail 4", "Location for Trail 4", 90, 6.5, "http://localhost/trail-4.jpg",
        [
            new(3, "Trail 4: Stage 1")
        ]);

        // Act
        await _repository.Update(trail4);

        // Assert
        var updated = await _context.Trails.Include(t => t.Route).FirstOrDefaultAsync(t => t.Id == Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"));
        Assert.NotNull(updated);
        updated.Name.Should().Be("Trail 4");
        updated.Description.Should().Be("Description for Trail 4");
        updated.ImageUrl.Should().Be("http://localhost/trail-1.jpg"); // Unchanged
        updated.Location.Should().Be("Location for Trail 4");
        updated.TimeInMinutes.Should().Be(90);
        updated.LengthInKilometers.Should().Be(6.5);
        updated.Route.Should().HaveCount(1);
        updated.Route[0].Stage.Should().Be(1);
        updated.Route[0].Description.Should().Be("Trail 4: Stage 1");
    }

    [Fact]
    public async Task UpdateImage_given_non_existing_id_does_nothing()
    {
        // Arrange
        var image = new TrailImage(Guid.Parse("c3b6927c-d294-4799-8865-31c5bf8bdc9b"), "http://localhost/trail-4.jpg");

        // Act
        await _repository.UpdateImage(image);

        // Assert
        var updated = await _context.Trails.FindAsync(Guid.Parse("c3b6927c-d294-4799-8865-31c5bf8bdc9b"));
        updated.Should().BeNull();
    }

    [Fact]
    public async Task UpdateImage_given_updates_image()
    {
        // Arrange
        var image = new TrailImage(Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"), "http://localhost/trail-5.jpg");

        // Act
        await _repository.UpdateImage(image);

        // Assert
        var updated = await _context.Trails.FindAsync(Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"));
        Assert.NotNull(updated);
        updated.ImageUrl.Should().Be("http://localhost/trail-5.jpg");
    }

    [Fact]
    public async Task UpdateImage_given_null_removes_image()
    {
        // Arrange
        var image = new TrailImage(Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"), default);

        // Act
        await _repository.UpdateImage(image);

        // Assert
        var updated = await _context.Trails.FindAsync(Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c"));
        Assert.NotNull(updated);
        updated.ImageUrl.Should().BeNull();
    }

    [Fact]
    public async Task Delete_given_existing_id_deletes()
    {
        // Arrange
        var id = Guid.Parse("e94aaaeb-0618-4285-a6bc-eb9dca83a88c");

        // Act
        await _repository.Delete(id);

        // Assert
        var count = await _context.Trails.CountAsync();
        count.Should().Be(1);

        var deleted = await _context.Trails.FindAsync(id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task Delete_given_non_existing_id_does_nothing()
    {
        // Arrange
        var id = Guid.Parse("c3b6927c-d294-4799-8865-31c5bf8bdc9b");

        // Act
        await _repository.Delete(id);

        // Assert
        var count = await _context.Trails.CountAsync();
        count.Should().Be(2);
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();

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

        _context.Trails.AddRange(trail1, trail2);
        await _context.SaveChangesAsync();
    }


    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}