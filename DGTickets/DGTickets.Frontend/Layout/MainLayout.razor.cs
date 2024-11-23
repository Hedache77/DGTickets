using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DGTickets.Frontend.Layout;

public partial class MainLayout
{
    private bool _drawerOpen = true;
    private string _icon = Icons.Material.Filled.DarkMode;
    private bool _darkMode { get; set; } = true;

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    MudTheme MyCustomTheme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            AppbarBackground = Colors.DeepOrange.Lighten2,
            DrawerBackground = Colors.Brown.Lighten3,
            Background = Colors.Gray.Lighten4
            
        },
        PaletteDark = new PaletteDark()
        {
            AppbarBackground = Colors.Blue.Darken4,
            DrawerBackground = Colors.BlueGray.Darken2,
            Background = Colors.Indigo.Lighten1
        },
    };

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void DarkModeToggle()
    {
        _darkMode = !_darkMode;
        _icon = _darkMode ? Icons.Material.Filled.LightMode : Icons.Material.Filled.DarkMode;
    }
}