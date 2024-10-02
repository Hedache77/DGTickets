using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.Entities;

public class Headquarter
{
    public int Id { get; set; }

    [Display(Name = "Headquarter", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    [Display(Name = "Address", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Address { get; set; } = null!;

    [Display(Name = "PhoneNumber", ResourceType = typeof(Literals))]
    [Phone(ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Email", ResourceType = typeof(Literals))]
    [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Email { get; set; } = null!;

    public City? City { get; set; }

    public int CityId { get; set; }

    public ICollection<Module>? Modules { get; set; }

    public int ModulesCount => Modules == null ? 0 : Modules.Count;

    public ICollection<HeadquarterMedicine>? HeadquarterMedicines { get; set; }

    public int MedicinesCount => HeadquarterMedicines == null ? 0 : HeadquarterMedicines.Count;
}