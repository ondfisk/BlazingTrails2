namespace BlazingTrails.Api.Endpoints;

public static class PutTrailEndpoint
{
    public const string RouteTemplate = "/api/v1/trails/{trailId}";

    public static async Task<Results<NoContent, ValidationProblem>> Invoke(ITrailsRepository repository, Guid trailId, TrailDTO trailDTO, TrailValidator validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(trailDTO, cancellationToken);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var trail = new Trail(trailId, trailDTO.Name, trailDTO.Description, trailDTO.Location, trailDTO.TimeInMinutes, trailDTO.LengthInKilometers, default, trailDTO.Route.Select(x => new RouteInstruction(x.Stage, x.Description)).ToList());

        await repository.Update(trail, cancellationToken);

        return TypedResults.NoContent();
    }
}
