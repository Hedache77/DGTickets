using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.Entities;

public class TicketMedicine
{
    public int Id { get; set; }

    public Ticket Ticket { get; set; } = null!;

    public int TicketId { get; set; }

    public MedicineStock Medicine { get; set; } = null!;

    public int MedicineId { get; set; }

    [Display(Name = "Quantity", ResourceType = typeof(Literals))]
    [Range(1, 1000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Quantity { get; set; }
}