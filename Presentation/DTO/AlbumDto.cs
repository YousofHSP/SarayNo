using Domain;

namespace Presentation.DTO;

public class AlbumDto : BaseDto<AlbumDto, Album>
{
    public string Title { get; set; }
    public int ProjectId { get; set; }
}