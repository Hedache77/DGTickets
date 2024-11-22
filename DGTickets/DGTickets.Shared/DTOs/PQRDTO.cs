using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.DTOs;

public class PQRDTO
{
    public int Id { get; set; }

    [Display(Name = "Code", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Code { get; set; } = null!;

    [Display(Name = "Description", ResourceType = typeof(Literals))]
    public string Description { get; set; } = null!;

    [Display(Name = "User", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string UserId { get; set; } = null!;
}