namespace BlazingTrails.Core;

public sealed record Trail(Guid Id, string Name, string Description, string Location, int TimeInMinutes, double LengthInKilometers, string? ImageUrl, IReadOnlyList<RouteInstruction> Route);

public sealed record RouteInstruction(int Stage, string Description);