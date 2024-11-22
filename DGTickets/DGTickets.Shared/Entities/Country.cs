using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.Entities;

public class Country
{
    public int Id { get; set; }

    [Display(Name = "Country", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string ImageFull => string.IsNullOrEmpty(Image) ? "/images/NoImage.png" : Image;

    public ICollection<State>? States { get; set; }

    public int StatesCount => States == null ? 0 : States.Count;
    public ICollection<User>? Users { get; set; }

    public int UsersCount => Users == null ? 0 : Users.Count;
}