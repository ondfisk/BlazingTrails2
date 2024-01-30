
namespace BlazingTrails.Infrastructure;

public class TrailsRepository(BlazingTrailsContext context) : ITrailsRepository
{
    public async Task Create(Trail trail, CancellationToken cancellationToken)
    {
        var entity = new TrailEntity
        {
            Id = trail.Id,
            Name = trail.Name,
            Description = trail.Description,
            Location = trail.Location,
            TimeInMinutes = trail.TimeInMinutes,
            LengthInKilometers = trail.LengthInKilometers,
            Route = trail.Route.Select(x => new RouteInstructionEntity { Stage = x.Stage, Description = x.Description }).ToList()
        };

        context.Trails.Add(entity);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Trail?> Read(Guid id, CancellationToken cancellationToken)
    {
        var trails = from t in context.Trails
                     where t.Id == id
                     select new Trail(t.Id, t.Name, t.Description, t.Location, t.TimeInMinutes, t.LengthInKilometers, t.Image, t.Route.Select(r => new RouteInstruction(r.Stage, r.Description)).ToList());

        return await trails.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Trail>> Read(CancellationToken cancellationToken)
    {
        var trails = from t in context.Trails
                     select new Trail(t.Id, t.Name, t.Description, t.Location, t.TimeInMinutes, t.LengthInKilometers, t.Image, t.Route.Select(r => new RouteInstruction(r.Stage, r.Description)).ToList());

        return await trails.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task Update(Trail trail, CancellationToken cancellationToken)
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
            entity.Route.Clear();

            foreach (var instruction in trail.Route)
            {
                entity.Route.Add(new RouteInstructionEntity { Stage = instruction.Stage, Description = instruction.Description });
            }

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task UpdateTrailImage(Guid id, string? image, CancellationToken cancellationToken)
    {
        var entity = await context.Trails.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (entity is not null)
        {
            entity.Image = image;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await context.Trails.FindAsync([id], cancellationToken).ConfigureAwait(false);

        if (entity is not null)
        {
            context.Trails.Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}