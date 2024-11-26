using CurrieTechnologies.Razor.SweetAlert2;
using DGTickets.Frontend.Repositories;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DGTickets.Frontend.Pages.Pqrs;

public partial class PQRCreate
{
    private PQRForm? pqrForm;
    private PQRDTO pqrDTO = new() { Code = "1", UserId = "1" };

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync<PQRDTO, PQR>("/api/pqrs/full", pqrDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        var pqr = responseHttp.Response;

        Return();
        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            Text = string.Format(Localizer["PQRCreated"], pqr!.Code),
            Icon = SweetAlertIcon.Info,
        });
    }

    private void Return()
    {
        pqrForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/pqrs");
    }
}