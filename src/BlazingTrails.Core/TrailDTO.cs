namespace BlazingTrails.Core;

public sealed class TrailDTO
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Location { get; set; } = "";
    public int TimeInMinutes { get; set; }
    public double LengthInKilometers { get; set; }
    public ICollection<RouteInstructionDTO> Route { get; set; } = [];

    public sealed class RouteInstructionDTO
    {
        public int Stage { get; set; }
        public string Description { get; set; } = "";
    }
}

public sealed class TrailValidator : AbstractValidator<TrailDTO>
{
    public TrailValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter a name").MaximumLength(100).WithMessage("Name must be less than 100 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter a description").MaximumLength(2000).WithMessage("Description must be less than 2000 characters");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Please enter a location").MaximumLength(50).WithMessage("Location must be less than 50 characters");
        RuleFor(x => x.TimeInMinutes).GreaterThan(0).WithMessage("Please enter a time");
        RuleFor(x => x.LengthInKilometers).GreaterThan(0).WithMessage("Please enter a length");
        RuleFor(x => x.LengthInKilometers).Must(x => x % 0.5 == 0).WithMessage("Fraction must be 0.0 or 0.5");
        RuleFor(x => x.Route).NotEmpty().WithMessage("Please add a route instruction");
        RuleForEach(x => x.Route).SetValidator(new RouteInstructionValidator());
    }
}

public sealed class RouteInstructionValidator : AbstractValidator<TrailDTO.RouteInstructionDTO>
{
    public RouteInstructionValidator()
    {
        RuleFor(x => x.Stage).NotEmpty().WithMessage("Please enter a stage");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter a description");
    }
}
