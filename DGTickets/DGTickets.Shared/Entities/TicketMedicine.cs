namespace DGTickets.Shared.Entities;

public class TicketMedicine
{
    public int Id { get; set; }

    public Ticket Ticket { get; set; } = null!;

    public int TicketId { get; set; }

    public MedicineStock Medicine { get; set; } = null!;

    public int MedicineId { get; set; }
}