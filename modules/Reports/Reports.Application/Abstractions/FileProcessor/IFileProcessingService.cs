using Microsoft.AspNetCore.Http;

namespace Reports.Application.Abstractions.FileProcessor;

public interface IFileProcessingService
{
    List<TModel> ProcessExcelFile<TModel>(IFormFile file, CancellationToken cancellationToken) where TModel : class, new();
    MemoryStream GenerateExcelFile<TModel>(List<TModel> data, CancellationToken cancellationToken) where TModel : class, new();
}
