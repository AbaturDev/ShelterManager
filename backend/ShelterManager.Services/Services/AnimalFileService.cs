using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Services.Constants;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Extensions;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class AnimalFileService : IAnimalFileService
{
    private readonly ShelterManagerContext _context;
    private readonly IFileService _fileService;
    
    public AnimalFileService(ShelterManagerContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task UploadAnimalProfileImageAsync(Guid id, string fileName, Stream imageStream, CancellationToken ct)
    {
        var animal = await _context.Animals
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (animal is null)
        {
            throw new NotFoundException("Animal not found");
        }
        
        var fileExtension = Path.GetExtension(fileName);
        
        var path = $"{AzureStoragePaths.ProfileImagesContainer}/{AzureStoragePaths.AnimalsContainer}/{id}{fileExtension}";

        if (animal.ImagePath is not null)
        {
            await _fileService.DeleteFileAsync(animal.ImagePath);
        }
        
        await _fileService.UploadFileAsync(imageStream, path);
        
        animal.ImagePath = path;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<FileStreamDto> GetAnimalProfileImageAsync(Guid id, CancellationToken ct)
    {
        var animal = await _context.Animals
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (animal is null)
        {
            throw new NotFoundException("Animal not found");
        }

        if (animal.ImagePath is null)
        {
            throw new BadRequestException("Animal does not have an profile image");
        }
        
        var image = await _fileService.GetFileStreamAsync(animal.ImagePath);

        if (image is null)
        {
            throw new NotFoundException("Image not found");
        }
        
        return new FileStreamDto
        {
            Stream = image,
            FileExtension = Path.GetExtension(animal.ImagePath)
        };
    }

    public async Task UploadAnimalFileAsync(Guid id, string fileName, Stream fileStream, CancellationToken ct)
    {
        var animalExists = await _context.Animals.AnyAsync(x => x.Id == id, ct);

        if (!animalExists)
        {
            throw new NotFoundException("Animal not found");
        }
        
        var path = $"{AzureStoragePaths.FilesContainer}/{AzureStoragePaths.AnimalsContainer}/{id}/{fileName}";
        await _fileService.UploadFileAsync(fileStream, path);
    }

    public async Task<PaginatedResponse<FileDto>> ListAnimalFilesAsync(Guid id, PageQueryFilter queryFilter, CancellationToken ct)
    {
        var animalExists = await _context.Animals.AnyAsync(x => x.Id == id, ct);

        if (!animalExists)
        {
            throw new NotFoundException("Animal not found");
        }

        var path = $"{AzureStoragePaths.FilesContainer}/{AzureStoragePaths.AnimalsContainer}/{id}";
        var files = await _fileService.ListFilesInFolderAsync(path);

        files = files.Where(x => x.IsFile).ToList();
        
        var totalCount = files.Count;
        
        var itemsDto = files
            .Select(x => new FileDto
            {
                Name = Path.GetFileName(x.Name),
            })
            .Paginate(queryFilter.Page, queryFilter.PageSize)
            .ToList();
        
        return new PaginatedResponse<FileDto>(itemsDto, queryFilter.Page, queryFilter.PageSize, totalCount);
    }

    public async Task<FileStreamDto> GetAnimalFileAsync(Guid id, string fileName, CancellationToken ct)
    {
        var animalExists = await _context.Animals.AnyAsync(x => x.Id == id, ct);

        if (!animalExists)
        {
            throw new NotFoundException("Animal not found");
        }

        var path = $"{AzureStoragePaths.FilesContainer}/{AzureStoragePaths.AnimalsContainer}/{id}/{fileName}";
        var file = await _fileService.GetFileStreamAsync(path);

        if (file is null)
        {
            throw new NotFoundException("File not found");
        }
        
        return new FileStreamDto
        {
            Stream = file,
            FileExtension = Path.GetExtension(fileName)
        };
    }

    public async Task DeleteAnimalFileAsync(Guid id, string fileName, CancellationToken ct)
    {
        var animalExists = await _context.Animals.AnyAsync(x => x.Id == id, ct);

        if (!animalExists)
        {
            throw new NotFoundException("Animal not found");
        }

        var path = $"{AzureStoragePaths.FilesContainer}/{AzureStoragePaths.AnimalsContainer}/{id}/{fileName}";
        var result = await _fileService.DeleteFileAsync(path);

        if (!result)
        {
            throw new NotFoundException("File not found");
        }
    }
}