using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DGTickets.Frontend;

public partial class App
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
}