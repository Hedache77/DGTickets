using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.Entities;

public class MedicineStock
{
    public int Id { get; set; }

    [Display(Name = "Medicine", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    [Display(Name = "Image", ResourceType = typeof(Literals))]
    public string? Image { get; set; }

    [Range(1, 1000)]
    [Required]
    public int Quantity { get; set; }

    [MaxLength(100)]
    [Required]
    public string Manufacturer { get; set; } = null!;

    [MaxLength(10)]
    [Required]
    public string UnitOfMeasure { get; set; } = null!;

    [Range(0.01, 1000)]
    [Required]
    public float QuantityPerUnit { get; set; }
}