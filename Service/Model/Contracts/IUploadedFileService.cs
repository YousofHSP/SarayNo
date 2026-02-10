
using Domain;
using Microsoft.AspNetCore.Http;

namespace Service.Model.Contracts
{
    public interface IUploadedFileService
    {
        Task SetDisableFilesAsync(CancellationToken ct, string modelType, int modelId, UploadedFileType? type);
        Task<string> UploadFileAsync(IFormFile file, int? albumId, string modelType, int modelId, int userId, CancellationToken ct, string description = "");
        Task<string> GetFilePath(string modelType, int modelId, UploadedFileType type, CancellationToken ct);
        string GetFilePath(UploadedFile model, CancellationToken ct);
        string GetUrl();
        Task<List<UploadedFile>> GetFiles(string modelType, List<int> modelIds, UploadedFileType? type, CancellationToken ct);
        Task<List<UploadedFile>> GetFiles(List<string > modelType, List<int> modelIds, UploadedFileType? type, CancellationToken ct);
        Task<List<UploadedFile>> GetFiles(int? albumId, CancellationToken ct);
        Task<UploadedFile> GetFile(int id,CancellationToken ct);
        Task RemoveFile(UploadedFile model, CancellationToken ct);
        Task RemoveFile(int id, CancellationToken ct);
        Task RemoveAlbumFiles(int albumId, CancellationToken ct);
        Task AddRangeAsync(List<UploadedFile> list, CancellationToken ct);
    }
}
