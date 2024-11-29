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
    public string Code { get; set; } = null!;

    [Display(Name = "TicketType", ResourceType = typeof(Literals))]
    public TicketType TicketType { get; set; }

    [Display(Name = "Headquarter", ResourceType = typeof(Literals))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int HeadquarterId { get; set; }

    [Display(Name = "User", ResourceType = typeof(Literals))]
    public string? UserId { get; set; }

    [Display(Name = "User", ResourceType = typeof(Literals))]
    public string? User { get; set; }

    [Display(Name = "Date", ResourceType = typeof(Literals))]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}")]
    public DateTime? OrderDate { get; set; }

    [Display(Name = "Date", ResourceType = typeof(Literals))]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}")]
    public DateTime? ServiceDate { get; set; }

    public string? Language { get; set; } = null!;
}