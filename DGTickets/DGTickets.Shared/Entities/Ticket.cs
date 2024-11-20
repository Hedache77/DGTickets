using DGTickets.Shared.Enums;
using DGTickets.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DGTickets.Shared.Entities;

public class Ticket
{
    public int Id { get; set; }

    [Display(Name = "Code", ResourceType = typeof(Literals))]
    [MaxLength(6, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Code { get; set; } = null!;

    [Display(Name = "TicketType", ResourceType = typeof(Literals))]
    public TicketType TicketType { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public Headquarter Headquarter { get; set; } = null!;

    [Display(Name = "Headquarter", ResourceType = typeof(Literals))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int HeadquarterId { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public Rating Rating { get; set; } = null!;

    [Display(Name = "Rating", ResourceType = typeof(Literals))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int RatingId { get; set; }
}