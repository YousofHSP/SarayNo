using Common.Exceptions;
using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Model.Contracts;

namespace Service.Model
{
    public class UploadedFileService : IUploadedFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IRepository<UploadedFile> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadedFileService(IWebHostEnvironment environment, IRepository<UploadedFile> repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _env = environment;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetFilePath(string modelType, int modelId, UploadedFileType type,
            CancellationToken ct)
        {
            var model = await _repository.TableNoTracking
                .FirstOrDefaultAsync(
                    i => i.ModelType == modelType && i.ModelId == modelId && i.Type == type && i.Status == true, ct);
            if (model is null)
                return "";
            return GetFilePath(model, ct);
        }

        public string GetFilePath(UploadedFile model, CancellationToken ct)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request is null)
                return "";
            if (model is not null)
                return $"{request.Scheme}://{request.Host.Host}:{request.Host.Port}/uploads/{model.SavedName}";
            return "";
        }

        public async Task<List<UploadedFile>> GetFiles(string modelType, List<int> modelIds, UploadedFileType? type,
            CancellationToken ct)
        {
            var query = _repository.Table
                .Where(i => i.ModelType == modelType && modelIds.Contains(i.ModelId))
                .AsQueryable();
            if (type is not null)
                query = query.Where(i => i.Type == type);
            return await query.ToListAsync(ct);
        }

        public async Task SetDisableFilesAsync(CancellationToken ct, string modelType, int modelId,
            UploadedFileType? type)
        {
            var query = _repository.Table
                .Where(i => i.ModelType == modelType && i.ModelId == modelId)
                .AsQueryable();
            if (type is not null)
                query = query.Where(i => i.Type == type);
            var list = await query.ToListAsync(ct);
            list = list.Select(i =>
            {
                i.Status = false;
                return i;
            }).ToList();
            await _repository.UpdateRangeAsync(list, ct);
        }

        public async Task<string> UploadFileAsync(IFormFile file, UploadedFileType type
            , string modelType, int modelId, int userId, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("فایل وارد نشد");
            await ValidateFile(file);

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var savedName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, savedName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath, ct);

            var uploadedFile = new UploadedFile
            {
                SavedName = savedName,
                OriginalName = file.FileName,
                Type = type,
                MimeType = file.ContentType,
                ModelType = modelType,
                ModelId = modelId,
                UserId = userId,
                Status = true
            };

            await _repository.AddAsync(uploadedFile, ct);
            _repository.Detach(uploadedFile);
            return filePath;
        }

        private async Task ValidateFile(IFormFile file)
        {
        }
    }
}