using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlazingTrails.Api.Endpoints;

public static class PutTrailImageEndpoint
{
    public const string RouteTemplate = "/api/v1/trails/{trailId}/image";

    public static async Task<Results<Created, ValidationProblem>> Invoke(HttpContext context, ITrailsRepository repository, Guid trailId, [FromForm] IFormFile file, FileValidator validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(file, cancellationToken);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var fileName = $"{trailId}.jpg";

        await SaveImage(file, fileName, cancellationToken);

        var imageUrl = GetImageUrl(context, fileName);

        var trailImage = new TrailImage(trailId, imageUrl);

        await repository.UpdateTrailImage(trailImage, cancellationToken);

        return TypedResults.Created(imageUrl);
    }

    private static string GetImageUrl(HttpContext context, string fileName)
    {
        var request = context.Request;
        var host = request.Host.ToUriComponent();
        var protocol = request.Scheme;
        var baseUrl = $"{protocol}://{host}";

        return $"{baseUrl}/Images/{fileName}";
    }

    private static async Task SaveImage(IFormFile file, string fileName, CancellationToken cancellationToken)
    {
        using var image = Image.Load(file.OpenReadStream());

        var resizeOptions = new ResizeOptions
        {
            Mode = ResizeMode.Pad,
            Size = new Size(640, 426)
        };
        image.Mutate(x => x.Resize(resizeOptions));

        var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "Images", fileName);

        await image.SaveAsJpegAsync(saveLocation, cancellationToken);
    }
}

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        RuleFor(x => x.Length).LessThan(1024 * 1024 * 2).WithMessage("File size must be less than 2MB");
        RuleFor(x => x.ContentType).Must(x => x == "image/jpeg" || x == "image/png").WithMessage("File must be a jpg or a png");
    }
}