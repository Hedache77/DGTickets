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

    public string Code { get; set; } = null!;

    [Display(Name = "TicketType", ResourceType = typeof(Literals))]
    public TicketType TicketType { get; set; }

    public Headquarter? Headquarter { get; set; }
    public int HeadquarterId { get; set; }

    public User? User { get; set; }
    public string? UserId { get; set; }

    [Display(Name = "Date", ResourceType = typeof(Literals))]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}")]
    public DateTime? OrderDate { get; set; }

    [Display(Name = "Date", ResourceType = typeof(Literals))]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}")]
    public DateTime? ServiceDate { get; set; }

    public ICollection<TicketMedicine>? TicketMedicines { get; set; }

    public int MedicinesCount => TicketMedicines == null ? 0 : TicketMedicines.Count;
}