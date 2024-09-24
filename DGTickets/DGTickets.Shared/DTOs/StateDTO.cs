using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.DTOs;

public class StateDTO
{
    public int Id { get; set; }

    [Display(Name = "State", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    [Display(Name = "Image", ResourceType = typeof(Literals))]
    public string? Image { get; set; }

    [Display(Name = "Country", ResourceType = typeof(Literals))]
    public int CountryId { get; set; }
}