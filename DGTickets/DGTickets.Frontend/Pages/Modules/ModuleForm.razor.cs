using CurrieTechnologies.Razor.SweetAlert2;
using DGTickets.Frontend.Repositories;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DGTickets.Frontend.Pages.Modules;

public partial class ModuleForm
{
    private EditContext editContext = null!;
    private Headquarter selectedHeadquarter = new();
    private List<Headquarter>? headquarters;

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    [EditorRequired, Parameter] public ModuleDTO ModuleDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;

    protected override void OnInitialized()
    {
        editContext = new(ModuleDTO);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadHeadquartersAsync();
    }

    private async Task LoadHeadquartersAsync()
    {
        var responseHttp = await Repository.GetAsync<List<Headquarter>>("/api/Headquarters/combo");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        headquarters = responseHttp.Response;
    }

    private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    {
        var formWasEdited = editContext.IsModified();

        if (!formWasEdited || FormPostedSuccessfully)
        {
            return;
        }

        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            Text = Localizer["LeaveAndLoseChanges"],
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            CancelButtonText = Localizer["Cancel"],
        });

        var confirm = !string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        context.PreventNavigation();
    }

    private async Task<IEnumerable<Headquarter>> SearchHeadquarter(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return headquarters!;
        }

        return headquarters!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private void HeadquarterChanged(Headquarter headquarter)
    {
        selectedHeadquarter = headquarter;
        ModuleDTO.HeadquarterId = headquarter.Id;
    }
}