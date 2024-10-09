using DGTickets.Frontend.Repositories;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DGTickets.Frontend.Pages.Headquarters;

public partial class AddMedicine
{
    private HeadquarterMedicineDTO? headquarterMedicineDTO;
    private AddMedicineForm? addMedicineForm;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        headquarterMedicineDTO = new HeadquarterMedicineDTO()
        {
            HeadquarterId = Id,
        };
    }

    private async Task AddAsync()
    {
        var responseHttp = await Repository.PostAsync("api/HeadquarterMedicines/full", headquarterMedicineDTO);

        if (responseHttp.Error)
        {
            var menssageError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[menssageError!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordCreatedOk"], Severity.Success);
    }

    private void Return()
    {
        addMedicineForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo($"/headquarter/medicines/{Id}");
    }
}