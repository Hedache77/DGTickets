﻿using DGTickets.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.DTOs;

public class CityDTO
{
    public int Id { get; set; }

    [Display(Name = "City", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    [Display(Name = "Image", ResourceType = typeof(Literals))]
    public string? Image { get; set; } // signo de ? quiere decir nullable, puede estar vacio

    [Display(Name = "State", ResourceType = typeof(Literals))]
    public int StateId { get; set; }
}