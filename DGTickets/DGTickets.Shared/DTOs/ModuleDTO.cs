using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.DTOs;

public class ModuleDTO
{
    public int Id { get; set; }

    [Display(Name = "Module", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    [Display(Name = "Headquarter", ResourceType = typeof(Literals))]
    public int HeadquarterId { get; set; }
}