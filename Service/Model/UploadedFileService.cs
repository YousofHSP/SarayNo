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
        //private readonly IWebHostEnvironment _env;
        private readonly IRepository<UploadedFile> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadedFileService(IRepository<UploadedFile> repository,
            IHttpContextAccessor httpContextAccessor)
        {
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

        public string GetUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request is null)
                return "";
            return $"{request.Scheme}://{request.Host.Host}:{request.Host.Port}/uploads/";
            
        }
        public string GetFilePath(UploadedFile model, CancellationToken ct)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request is null)
                return "";
            if (model is not null)
                return $"{request.Scheme}://{request.Host.Host}/uploads/{model.SavedName}";
            return "";
        }

        public async Task<List<UploadedFile>> GetFiles(string modelType, List<int> modelIds, UploadedFileType? type,
            CancellationToken ct)
        {
            var query = _repository.Table
                .Where(i => i.ModelType == modelType)
                .AsQueryable();
            if (type is not null)
                query = query.Where(i => i.Type == type);
            if (modelIds.Any())
                query = query
                    .Where(i => modelIds.Contains(i.ModelId));
            return await query.ToListAsync(ct);
        }

        public async Task<List<UploadedFile>> GetFiles(List<string> modelType, List<int> modelIds, UploadedFileType? type, CancellationToken ct)
        {
            var query = _repository.Table
                .Where(i => modelType.Contains(i.ModelType))
                .AsQueryable();
            if (type is not null)
                query = query.Where(i => i.Type == type);
            if (modelIds.Any())
                query = query
                    .Where(i => modelIds.Contains(i.ModelId));
            return await query.ToListAsync(ct);
        }

        public async Task<List<UploadedFile>> GetFiles(int? albumId, CancellationToken ct)
        {
            var res = await _repository.TableNoTracking
                .Where(i => i.AlbumId == albumId)
                .ToListAsync(ct);

            return res;
        }

        public async Task<UploadedFile> GetFile(int id, CancellationToken ct)
        {
            var model = await _repository.TableNoTracking.FirstOrDefaultAsync(i => i.Id == id, ct);
            if (model is null)
                throw new NotFoundException();
            return model;
        }

        public async Task RemoveFile(UploadedFile model, CancellationToken ct)
        {
            
            var filePath = Path.Combine("wwwroot", "uploads", model.SavedName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await _repository.DeleteAsync(model, ct);
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

        public async Task<string> UploadFileAsync(IFormFile file,int? albumId 
            , string modelType, int modelId, int userId, CancellationToken ct, string description = "")
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("فایل وارد نشد");
            await ValidateFile(file);

            var uploadsFolder = Path.Combine("wwwroot", "uploads");
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
                AlbumId = albumId,
                OriginalName = file.FileName,
                Type = UploadedFileType.Unknown,
                MimeType = file.ContentType,
                ModelType = modelType,
                ModelId = modelId,
                UserId = userId,
                Status = true,
                Description = description
            };

            await _repository.AddAsync(uploadedFile, ct);
            _repository.Detach(uploadedFile);
            return filePath;
        }

        public async Task AddRangeAsync(List<UploadedFile> list, CancellationToken ct)
        {
            await _repository.AddRangeAsync(list, ct);
        }

        private async Task ValidateFile(IFormFile file)
        {
        }
    }
}