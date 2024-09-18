using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DGTickets.Frontend.Layout;

public partial class MainLayout
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
}