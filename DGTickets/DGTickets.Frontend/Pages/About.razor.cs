using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DGTickets.Frontend.Pages;

public partial class About
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
}