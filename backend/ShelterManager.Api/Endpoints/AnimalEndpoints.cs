using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Core.Exceptions;
using ShelterManager.Services.Dtos.Animals;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class AnimalEndpoints
{
    public static RouteGroupBuilder MapAnimalEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.AnimalRoute, apiVersion);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .RequireAuthorization(AuthorizationPolicies.MustChangePasswordPolicyName)
            .RequireAuthorization(AuthorizationPolicies.UserPolicyName)
            .WithTags(nameof(AnimalEndpoints));

        group.MapGet("", ListAnimals)
            .WithRequestValidation<PageQueryFilter>();
        group.MapGet("/{id:guid}", GetAnimal);
        group.MapPost("", CreateAnimal)
            .WithRequestValidation<CreateAnimalDto>();
        group.MapPut("/{id:guid}", UpdateAnimal)
            .WithRequestValidation<UpdateAnimalDto>();
        group.MapDelete("/{id:guid}", DeleteAnimal);

        group.MapPost("/{id:guid}/profile-image", UploadProfileImage)
            .DisableAntiforgery();
        group.MapGet("/{id:guid}/profile-image", GetProfileImage);
        group.MapGet("/{id:guid}/files/{fileName}", GetFile);
        group.MapGet("/{id:guid}/files", ListFiles)
            .WithRequestValidation<PageQueryFilter>();
        group.MapPost("/{id:guid}/files", UploadFile)
            .DisableAntiforgery();
        group.MapDelete("/{id:guid}/files/{fileName}", DeleteFile);
        
        return group;
    }

    private static async Task<Ok<PaginatedResponse<AnimalDto>>> ListAnimals(
        [AsParameters] AnimalPageQueryFilter pageQueryFilter,
        [FromServices] IAnimalService animalService,
        CancellationToken ct)
    {
        var response = await animalService.ListAnimalsAsync(pageQueryFilter, ct);

        return TypedResults.Ok(response);
    }

    private static async Task<Ok<AnimalDto>> GetAnimal(
        Guid id,
        [FromServices] IAnimalService animalService,
        CancellationToken ct)
    {
        var response = await animalService.GetAnimalByIdAsync(id, ct);

        return TypedResults.Ok(response);
    }

    private static async Task<Created> CreateAnimal(
        [FromBody] CreateAnimalDto animalDto,
        [FromServices] IAnimalService animalService,
        CancellationToken ct
    )
    {
        var id = await animalService.CreateAnimalAsync(animalDto, ct);

        return TypedResults.Created($"/{ApiRoutes.AnimalRoute}/{id}");
    }

    private static async Task<NoContent> DeleteAnimal(
        Guid id,
        [FromServices] IAnimalService animalService,
        CancellationToken ct
    )
    {
        await animalService.DeleteAnimalAsync(id, ct);

        return TypedResults.NoContent();
    }

    private static async Task<Ok> UpdateAnimal(
        Guid id,
        [FromBody] UpdateAnimalDto dto,
        [FromServices] IAnimalService animalService,
        CancellationToken ct
    )
    {
        await animalService.UpdateAnimalAsync(id, dto, ct);

        return TypedResults.Ok();
    }

    private static async Task<Ok> UploadProfileImage(
        Guid id,
        [FromForm] IFormFile file,
        [FromServices] IAnimalFileService animalFileService,
        CancellationToken ct)
    {
        var extension = Path.GetExtension(file.FileName);
        if (!FileExtensionUtils.IsValidImageExtension(extension))
        {
            throw new BadRequestException("File format is not supported");
        }
        
        await using var stream = file.OpenReadStream();
        await animalFileService.UploadAnimalProfileImageAsync(id, file.FileName, stream, ct);
        
        return TypedResults.Ok();
    }
    
    private static async Task<IResult> GetProfileImage(
        Guid id,
        [FromServices] IAnimalFileService animalFileService,
        CancellationToken ct)
    {
        var fileStreamDto = await animalFileService.GetAnimalProfileImageAsync(id, ct);
        
        var contentType = FileExtensionUtils.MapToContentType(fileStreamDto.FileExtension);
        
        return Results.File(fileStreamDto.Stream, contentType);
    }
    
    private static async Task<Ok> UploadFile(
        Guid id,
        [FromForm] IFormFile file,
        [FromServices] IAnimalFileService animalFileService,
        CancellationToken ct)
    {
        await using var stream = file.OpenReadStream();
        await animalFileService.UploadAnimalFileAsync(id, file.FileName, stream, ct);
        
        return TypedResults.Ok();
    }
    
    private static async Task<IResult> GetFile(
        Guid id,
        [FromRoute] string fileName,
        [FromServices] IAnimalFileService animalFileService,
        CancellationToken ct)
    {
        var fileStreamDto = await animalFileService.GetAnimalFileAsync(id, fileName, ct);
        
        var contentType = FileExtensionUtils.MapToContentType(fileStreamDto.FileExtension);
        
        return Results.File(fileStreamDto.Stream, contentType);
    }
    
    private static async Task<NoContent> DeleteFile(
        Guid id,
        string fileName,
        [FromServices] IAnimalFileService animalFileService,
        CancellationToken ct)
    {
        await animalFileService.DeleteAnimalFileAsync(id, fileName, ct);
        
        return TypedResults.NoContent();
    }
    
    private static async Task<Ok<PaginatedResponse<FileDto>>> ListFiles(
        Guid id,
        [AsParameters] PageQueryFilter pageQueryFilter,
        [FromServices] IAnimalFileService animalFileService,
        CancellationToken ct)
    {
        var response = await animalFileService.ListAnimalFilesAsync(id, pageQueryFilter, ct);
        
        return TypedResults.Ok(response);
    }
}