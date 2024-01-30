namespace BlazingTrails.Api.Endpoints;

public static class PostTrailEndpoint
{
    public const string RouteTemplate = "/api/v1/trails";

    public static async Task<Results<Created<Trail>, ValidationProblem>> Invoke(ITrailsRepository repository, TrailDTO trailDTO, TrailValidator validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(trailDTO, cancellationToken);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        // To test server-side validation uncomment the following line
        //return TypedResults.ValidationProblem(new Dictionary<string, string[]> { { "Name", new[] { "Name already exists" } } });

        var trail = new Trail(trailDTO.Id, trailDTO.Name, trailDTO.Description, trailDTO.Location, trailDTO.TimeInMinutes, trailDTO.LengthInKilometers, default, trailDTO.Route.Select(x => new RouteInstruction(x.Stage, x.Description)).ToList());

        await repository.Create(trail, cancellationToken);

        return TypedResults.Created($"{RouteTemplate}/{trail.Id}", trail);
    }
}
