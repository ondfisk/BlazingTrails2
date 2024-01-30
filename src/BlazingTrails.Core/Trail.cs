namespace BlazingTrails.Core;

public record Trail(Guid Id, string Name, string Description, string Location, int TimeInMinutes, double LengthInKilometers, string? Image, ICollection<RouteInstruction> Route);

public record RouteInstruction(int Stage, string Description);