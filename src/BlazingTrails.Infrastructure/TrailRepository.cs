
namespace BlazingTrails.Infrastructure;

public sealed class TrailRepository(BlazingTrailsContext context) : ITrailRepository
{
    public async Task Create(Trail trail, CancellationToken cancellationToken = default)
    {
        var entity = new TrailEntity
        {
            Id = trail.Id,
            Name = trail.Name,
            Description = trail.Description,
            Location = trail.Location,
            TimeInMinutes = trail.TimeInMinutes,
            LengthInKilometers = trail.LengthInKilometers
        };
        var stage = 0;
        foreach (var route in trail.Route.OrderBy(r => r.Stage))
        {
            entity.Route.Add(new RouteInstructionEntity { Stage = ++stage, Description = route.Description });
        }

        context.Trails.Add(entity);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Trail?> Read(Guid id, CancellationToken cancellationToken = default)
    {
        var trails = from t in context.Trails
                     where t.Id == id
                     select new Trail(t.Id, t.Name, t.Description, t.Location, t.TimeInMinutes, t.LengthInKilometers, t.ImageUrl, t.Route.Select(r => new RouteInstruction(r.Stage, r.Description)).ToList());

        return await trails.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Trail>> Read(CancellationToken cancellationToken = default)
    {
        var trails = from t in context.Trails
                     orderby t.Name
                     select new Trail(t.Id, t.Name, t.Description, t.Location, t.TimeInMinutes, t.LengthInKilometers, t.ImageUrl, t.Route.Select(r => new RouteInstruction(r.Stage, r.Description)).ToList());

        return await trails.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task Update(Trail trail, CancellationToken cancellationToken = default)
    {
        var entity = await context.Trails.Include(t => t.Route).FirstOrDefaultAsync(t => t.Id == trail.Id, cancellationToken).ConfigureAwait(false);

        if (entity is not null)
        {
            entity.Id = trail.Id;
            entity.Name = trail.Name;
            entity.Description = trail.Description;
            entity.Location = trail.Location;
            entity.TimeInMinutes = trail.TimeInMinutes;
            entity.LengthInKilometers = trail.LengthInKilometers;

            foreach (var route in entity.Route)
            {
                context.Remove(route);
            }
            var stage = 0;
            foreach (var route in trail.Route)
            {
                entity.Route.Add(new RouteInstructionEntity { Stage = ++stage, Description = route.Description });
            }

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task UpdateImage(TrailImage trailImage, CancellationToken cancellationToken = default)
    {
        var entity = await context.Trails.FindAsync([trailImage.TrailId], cancellationToken).ConfigureAwait(false);

        if (entity is not null)
        {
            entity.ImageUrl = trailImage.ImageUrl;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Trails.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (entity is not null)
        {
            context.Trails.Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}