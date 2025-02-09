namespace API.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using API.DataEntities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    //EF Navigations properties
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}