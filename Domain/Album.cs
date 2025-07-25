using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

public class Album : BaseEntity
{
    public string Title { get; set; }

    public List<UploadedFile>? UploadedFiles { get; set; }
}

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.HasMany(i => i.UploadedFiles)
            .WithOne(i => i.Album)
            .HasForeignKey(i => i.AlbumId);

    }
}