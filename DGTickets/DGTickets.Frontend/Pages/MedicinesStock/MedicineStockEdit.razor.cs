using CurrieTechnologies.Razor.SweetAlert2;
using DGTickets.Frontend.Repositories;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using static MudBlazor.Colors;

namespace DGTickets.Frontend.Pages.MedicinesStock;

public partial class MedicineStockEdit
{
    private MedicineStock? medicineStock;
    private MedicineStockForm? medicineStockForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<MedicineStock>($"api/MedicinesStock/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("medicines");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(Localizer["Error"], messageError, SweetAlertIcon.Error);
            }
        }
        else
        {
            var medicine = responseHttp.Response;
            medicineStock = new MedicineStock()
            {
                Id = medicine!.Id,
                Name = medicine!.Name,
                Image = medicine.Image,
                Quantity = medicine.Quantity,
                Manufacturer = medicine.Manufacturer,
                UnitOfMeasure = medicine.UnitOfMeasure,
                QuantityPerUnit = medicine.QuantityPerUnit,
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/MedicinesStock/full", medicineStock);

        if (responseHttp.Error)
        {
            var mensajeError = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync(Localizer["Error"], Localizer[mensajeError!], SweetAlertIcon.Error);
            return;
        }

        Return();
        var toast = SweetAlertService.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["RecordSavedOk"]);
    }

    private void Return()
    {
        medicineStockForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("medicines");
    }
}