using CurrieTechnologies.Razor.SweetAlert2;
using DGTickets.Frontend.Repositories;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using DGTickets.Shared.Enums;
using Microsoft.AspNetCore.Components.Authorization;

namespace DGTickets.Frontend.Pages.Tickets;

public partial class TicketForm
{
    private EditContext editContext = null!;
    private Headquarter selectedHeadquarter = new();
    private Rating selectedRating = new();
    private List<Headquarter>? headquarters;
    private bool esAdmin;
    private TicketType ticketType { get; set; }

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    [EditorRequired, Parameter] public TicketDTO TicketDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    public bool FormPostedSuccessfully { get; set; } = false;

    protected override async void OnInitialized()
    {
        editContext = new(TicketDTO);
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        esAdmin = authState.User.IsInRole("Admin");
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadHeadquarterAsync();
    }

    private async Task LoadHeadquarterAsync()
    {
        TicketDTO.Code = "ABC123";
        TicketDTO.User = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "emailUser");

        var responseHttp = await Repository.GetAsync<List<Headquarter>>("/api/headquarters/combo");
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
        TicketDTO.HeadquarterId = headquarter.Id;
    }
}