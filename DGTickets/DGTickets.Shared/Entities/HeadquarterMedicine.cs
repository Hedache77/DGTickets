namespace DGTickets.Shared.Entities;

public class HeadquarterMedicine
{
    public int Id { get; set; }

    public Headquarter Headquarter { get; set; } = null!;

    public int HeadquarterId { get; set; }

    public MedicineStock Medicine { get; set; } = null!;

    public int MedicineId { get; set; }
}