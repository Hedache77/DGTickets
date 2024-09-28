using DGTickets.Frontend.Pages.Headquarters;
using DGTickets.Frontend.Repositories;
using DGTickets.Shared.DTOs;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace DGTickets.Frontend.Pages.Modules;

public partial class ModuleEdit
{
    private ModuleDTO? moduleDTO;
    private ModuleForm? moduleForm;
    private Headquarter selectedHeadquarter = new();

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await Repository.GetAsync<Module>($"api/Modules/{Id}");

        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("modules");
            }
            else
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(messageError!, Severity.Error);
            }
        }
        else
        {
            var module = responseHttp.Response;
            moduleDTO = new ModuleDTO()
            {
                Id = module!.Id,
                Name = module!.Name,
                HeadquarterId = module.HeadquarterId,
            };
            selectedHeadquarter = module.Headquarter!;
        }
    }

    private async Task EditAsync()
    {
        var responseHttp = await Repository.PutAsync("api/Modules/full", moduleDTO);

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
        moduleForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("modules");
    }
}