
using Domain;
using Microsoft.AspNetCore.Http;

namespace Service.Model.Contracts
{
    public interface IUploadedFileService
    {
        Task SetDisableFilesAsync(CancellationToken ct, string modelType, int modelId, UploadedFileType? type);
        Task<string> UploadFileAsync(IFormFile file, UploadedFileType type, string modelType, int modelId, int userId, CancellationToken ct, string description = "");
        Task<string> GetFilePath(string modelType, int modelId, UploadedFileType type, CancellationToken ct);
        string GetFilePath(UploadedFile model, CancellationToken ct);
        Task<List<UploadedFile>> GetFiles(string modelType, List<int> modelIds, UploadedFileType? type, CancellationToken ct);
        Task<UploadedFile> GetFile(int id,CancellationToken ct);
        Task RemoveFile(UploadedFile model, CancellationToken ct);
    }
}
