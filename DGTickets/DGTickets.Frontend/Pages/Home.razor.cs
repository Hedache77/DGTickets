using DGTickets.Frontend.Pages.Tickets;
using DGTickets.Frontend.Repositories;
using DGTickets.Frontend.Shared;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Linq;

namespace DGTickets.Frontend.Pages;

public partial class Home
{
    private const string baseUrl = "api/MedicinesStock";
    private bool loading;
    private List<MedicineStock>? MedicineStock { get; set; }
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
    private bool arrows = true;
    private bool bullets = true;
    private bool enableSwipeGesture = true;
    private bool autocycle = true;
    private Transition transition = Transition.Fade;

    protected override async Task OnInitializedAsync()
    {
        await LoadTotalRecordsAsync();
    }

    private async Task LoadTotalRecordsAsync()
    {
        loading = true;
        var url = $"{baseUrl}";

        var responseHttp = await Repository.GetAsync<List<MedicineStock>>(url);

        MedicineStock = responseHttp.Response.Where(medicine => medicine.Quantity <= 1).ToList();

        loading = false;
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        IDialogReference? dialog;
        if (isEdit)
        {
            var parameters = new DialogParameters
                {
                    { "Id", id }
                };
            dialog = DialogService.Show<TicketEdit>($"{Localizer["Edit"]} {Localizer["Ticket"]}", parameters, options);
        }
        else
        {
            dialog = DialogService.Show<TicketCreate>($"{Localizer["New"]} {Localizer["Ticket"]}", options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();
            NavigationManager.NavigateTo("/tickets");
        }
    }
}