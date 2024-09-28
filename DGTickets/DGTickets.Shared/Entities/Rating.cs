using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.Entities;

public class Rating
{
    public int Id { get; set; }

    [Display(Name = "Code", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Code { get; set; } = null!;

    [Display(Name = "Rating", ResourceType = typeof(Literals))]
    [Range(1, 5, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Value { get; set; }

    [Display(Name = "Description", ResourceType = typeof(Literals))]
    public string Description { get; set; } = null!;
}