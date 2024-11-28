using DGTickets.Frontend.Repositories;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DGTickets.Frontend.Pages.Headquarters;

public partial class HeadquarterEdit
{
    private HeadquarterDTO? headquarterDTO;
    private HeadquarterForm? headquarterForm;
    private City selectedCity = new();

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<Headquarter>($"api/Headquarters/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("headquarters");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            var headquarter = responseHttp.Response;
            headquarterDTO = new HeadquarterDTO()
            {
                Id = headquarter!.Id,
                Name = headquarter!.Name,
                CityId = headquarter.CityId,
                Address = headquarter.Address,
                PhoneNumber = headquarter.PhoneNumber,
                Email = headquarter.Email
            };
            selectedCity = headquarter.City!;
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/Headquarters/full", headquarterDTO);

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
        headquarterForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("headquarters");
    }
}