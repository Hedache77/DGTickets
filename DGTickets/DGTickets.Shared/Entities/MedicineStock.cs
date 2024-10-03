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

    [Display(Name = "Quantity", ResourceType = typeof(Literals))]
    [Range(1, 1000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Quantity { get; set; }

    [Display(Name = "Manufacturer", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Manufacturer { get; set; } = null!;

    [Display(Name = "UnitOfMeasure", ResourceType = typeof(Literals))]
    [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string UnitOfMeasure { get; set; } = null!;

    [Display(Name = "QuantityPerUnit", ResourceType = typeof(Literals))]
    [Range(0.01, 1000, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public float QuantityPerUnit { get; set; }

    public string ImageFull => string.IsNullOrEmpty(Image) ? "/images/noImage.png" : Image;

    public bool IsImageSquare { get; set; }

    public ICollection<HeadquarterMedicine>? HeadquarterMedicines { get; set; }

    public int HeadquartersCount => HeadquarterMedicines == null ? 0 : HeadquarterMedicines.Count;
}