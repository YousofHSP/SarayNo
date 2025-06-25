using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain
{
    public class UploadedFile : BaseEntity
    {
        public string SavedName { get; set; }
        public string OriginalName { get; set; }
        public string MimeType { get; set; }
        public string ModelType { get; set; }
        public int ModelId { get; set; }
        public string Hash { get; set; }
        public string SaltCode { get; set; }
        public int UserId { get; set; }
        public UploadedFileType Type { get; set; }
        public bool Status { get; set; }

        public User User { get; set; }
    }
    
    public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.HasOne(i => i.User)
                .WithMany(i => i.UploadedFiles)
                .HasForeignKey(i => i.UserId);
        }
    }

    public enum UploadedFileType
    {
        Unknown = 0,
        Avatar = 1,
    }
}

