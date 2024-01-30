namespace BlazingTrails.Api.Endpoints;

public static class PostTrailEndpoint
{
    public const string RouteTemplate = "/api/v1/trails";

    public static async Task<Results<Created<Trail>, ValidationProblem>> Invoke(TrailDTO trailDTO, TrailValidator validator, ITrailRepository repository, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(trailDTO, cancellationToken);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var trailId = Guid.NewGuid();
        var trail = new Trail(trailId, trailDTO.Name, trailDTO.Description, trailDTO.Location, trailDTO.TimeInMinutes, trailDTO.LengthInKilometers, default, trailDTO.Route.Select(x => new RouteInstruction(x.Stage, x.Description)).ToList());

        await repository.Create(trail, cancellationToken);

        return TypedResults.Created($"{RouteTemplate}/{trail.Id}", trail);
    }
}
