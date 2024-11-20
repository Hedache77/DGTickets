using DGTickets.Shared.Entities;
using DGTickets.Shared.Enums;
using DGTickets.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGTickets.Shared.DTOs;

public class TicketDTO
{
    public int Id { get; set; }

    [Display(Name = "Code", ResourceType = typeof(Literals))]
    [MaxLength(6, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Code { get; set; } = null!;

    [Display(Name = "TicketType", ResourceType = typeof(Literals))]
    public TicketType TicketType { get; set; }

    [Display(Name = "Headquarter", ResourceType = typeof(Literals))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int HeadquarterId { get; set; }

    [Display(Name = "Rating", ResourceType = typeof(Literals))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int RatingId { get; set; }
}