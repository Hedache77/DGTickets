using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.Entities;

public class Module
{
    public int Id { get; set; }

    [Display(Name = "Module", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public Headquarter? Headquarter { get; set; }

    public int HeadquarterId { get; set; }
}