using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain
{
    public class UploadedFile : BaseEntity
    {

        public int? AlbumId { get; set; }
        public string SavedName { get; set; }
        public string OriginalName { get; set; }
        public string MimeType { get; set; }
        public string ModelType { get; set; }
        public int ModelId { get; set; }
        public int UserId { get; set; }
        public UploadedFileType Type { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }

        public User User { get; set; }
        public Album? Album{ get; set; }
    }
    
    public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.Property(i => i.Description).HasDefaultValue("");
            builder.HasOne(i => i.User)
                .WithMany(i => i.UploadedFiles)
                .HasForeignKey(i => i.UserId);
            builder.HasOne(i => i.Album)
                .WithMany(i => i.UploadedFiles)
                .HasForeignKey(i => i.AlbumId);
        }
    }

    public enum UploadedFileType
    {
        [Display(Name = "مشخص نشده")]
        Unknown,
        [Display(Name = "پروفایل")]
        Avatar
    }
}

