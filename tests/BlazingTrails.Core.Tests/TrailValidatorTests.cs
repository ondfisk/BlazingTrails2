using FluentValidation.TestHelper;

namespace BlazingTrails.Core.Tests;

public sealed class TrailValidatorTests
{
    private readonly TrailValidator _validator = new();

    [Fact]
    public void Trail_given_TrailId_and_Image_is_valid()
    {
        var trail = new TrailDTO
        {
            Id = Guid.NewGuid(),
            Name = "Trail",
            Description = "Description",
            Location = "Location",
            TimeInMinutes = 60,
            LengthInKilometers = 5.0,
            Route =
            [
                new TrailDTO.RouteInstructionDTO()
                {
                    Stage = 1,
                    Description = "Description"
                }
            ]
        };

        var result = _validator.TestValidate(trail);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void TrailImage_given_negative_LengthInKilometers_is_not_valid()
    {
        var trail = new TrailDTO
        {
            Id = Guid.NewGuid(),
            Name = "Trail",
            Description = "Description",
            Location = "Location",
            TimeInMinutes = 60,
            LengthInKilometers = -1.0,
            Route =
            [
                new TrailDTO.RouteInstructionDTO()
                {
                    Stage = 1,
                    Description = "Description"
                }
            ]
        };

        var result = _validator.TestValidate(trail);

        result.ShouldHaveValidationErrorFor(i => i.LengthInKilometers).WithErrorMessage("Please enter a length");
    }

    [Fact]
    public void TrailImage_given_non_half_fractional_LengthInKilometers_is_not_valid()
    {
        var trail = new TrailDTO
        {
            Id = Guid.NewGuid(),
            Name = "Trail",
            Description = "Description",
            Location = "Location",
            TimeInMinutes = 60,
            LengthInKilometers = 5.25,
            Route =
            [
                new TrailDTO.RouteInstructionDTO()
                {
                    Stage = 1,
                    Description = "Description"
                }
            ]
        };

        var result = _validator.TestValidate(trail);

        result.ShouldHaveValidationErrorFor(i => i.LengthInKilometers).WithErrorMessage("Fraction must be 0.0 or 0.5");
    }

    [Fact]
    public void TrailImage_given_half_fractional_LengthInKilometers_is_valid()
    {
        var trail = new TrailDTO
        {
            Id = Guid.NewGuid(),
            Name = "Trail",
            Description = "Description",
            Location = "Location",
            TimeInMinutes = 60,
            LengthInKilometers = 2.5,
            Route =
            [
                new TrailDTO.RouteInstructionDTO()
                {
                    Stage = 1,
                    Description = "Description"
                }
            ]
        };

        var result = _validator.TestValidate(trail);

        result.ShouldNotHaveAnyValidationErrors();
    }
}