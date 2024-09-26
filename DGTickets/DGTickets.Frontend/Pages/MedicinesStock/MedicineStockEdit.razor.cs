using DGTickets.Frontend.Repositories;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DGTickets.Frontend.Pages.MedicinesStock;

public partial class MedicineStockEdit
{
    private MedicineStock? medicineStock;
    private MedicineStockForm? medicineStockForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
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
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            var medicine = responseHttp.Response;
            medicineStock = new MedicineStock()
            {
                Id = medicine!.Id,
                Name = medicine.Name,
                Image = medicine.Image,
                IsImageSquare = medicine.IsImageSquare,
                Quantity = medicine.Quantity,
                Manufacturer = medicine.Manufacturer,
                UnitOfMeasure = medicine.UnitOfMeasure,
                QuantityPerUnit = medicine.QuantityPerUnit
            };
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/MedicinesStock/full", medicineStock);

        if (responseHttp.Error)
        {
            var messageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[messageError!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);
    }

    private void Return()
    {
        medicineStockForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("medicines");
    }
}