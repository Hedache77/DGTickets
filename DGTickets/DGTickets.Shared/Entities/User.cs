﻿using DGTickets.Shared.Enums;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DGTickets.Shared.Entities;

public class User : IdentityUser
{
    [Display(Name = "FirstName", ResourceType = typeof(Literals))]
    [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string FirstName { get; set; } = null!;

    [Display(Name = "LastName", ResourceType = typeof(Literals))]
    [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string LastName { get; set; } = null!;

    [Display(Name = "Image", ResourceType = typeof(Literals))]
    public string? Photo { get; set; }

    [Display(Name = "UserType", ResourceType = typeof(Literals))]
    public UserType UserType { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public Country Country { get; set; } = null!;

    [Display(Name = "Country", ResourceType = typeof(Literals))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int CountryId { get; set; }

    [Display(Name = "User", ResourceType = typeof(Literals))]
    public string FullName => $"{FirstName} {LastName}";

    public string PhotoFull => string.IsNullOrEmpty(Photo) ? "/images/NoImage.png" : Photo;

    public ICollection<Ticket>? Tickets { get; set; }

    public int TicketsCount => Tickets == null ? 0 : Tickets.Count;

    public ICollection<PQR>? PQRs { get; set; }

    public int PQRsCount => PQRs == null ? 0 : PQRs.Count;
}