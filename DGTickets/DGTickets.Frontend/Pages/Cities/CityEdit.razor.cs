using DGTickets.Frontend.Repositories;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using static MudBlazor.Colors;

namespace DGTickets.Frontend.Pages.Cities;

public partial class CityEdit
{
    private CityDTO? cityDTO;
    private CityForm? cityForm;
    private State selectedState = new();

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<City>($"api/cities/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("cities");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            var city = responseHttp.Response;
            cityDTO = new CityDTO()
            {
                Id = city!.Id,
                Name = city!.Name,
                Image = city.Image,
                StateId = city.StateId
            };
            selectedState = city.State!;
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/cities/full", cityDTO);

        if (responseHttp.Error)
        {
            var mensajeError = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[mensajeError!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordSavedOk"], Severity.Success);
    }

    private void Return()
    {
        cityForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("cities");
    }
}