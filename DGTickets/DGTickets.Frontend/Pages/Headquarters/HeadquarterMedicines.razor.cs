using DGTickets.Frontend.Repositories;
using DGTickets.Frontend.Shared;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;

namespace DGTickets.Frontend.Pages.Headquarters;

[Authorize(Roles = "Admin")]
public partial class HeadquarterMedicines
{
    private Headquarter? headquarter;
    private List<HeadquarterMedicine>? headquarterMedicines;

    private MudTable<HeadquarterMedicine> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrlHeadquarter = "api/Headquarters";
    private const string baseUrlHeadquarterMedicine = "api/HeadquarterMedicines";
    private string infoFormat = "{first_item}-{last_item} de {all_items}";

    [Parameter] public int HeadquarterId { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        await LoadTotalRecords();
    }

    private async Task<bool> LoadHeadquarterAsync()
    {
        var responseHttp = await Repository.GetAsync<Headquarter>($"{baseUrlHeadquarter}/{HeadquarterId}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/headquarters");
                return false;
            }

            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return false;
        }
        headquarter = responseHttp.Response;
        return true;
    }

    private async Task<bool> LoadTotalRecords()
    {
        loading = true;
        if (headquarter is null)
        {
            var ok = await LoadHeadquarterAsync();
            if (!ok)
            {
                NoData();
                return false;
            }
        }

        var url = $"{baseUrlHeadquarterMedicine}/totalRecordsPaginated/?id={HeadquarterId}";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }
        var responseHttp = await Repository.GetAsync<int>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return false;
        }
        totalRecords = responseHttp.Response;
        loading = false;
        return true;
    }

    private async Task<TableData<HeadquarterMedicine>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{baseUrlHeadquarterMedicine}/paginated?id={HeadquarterId}&page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<HeadquarterMedicine>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return new TableData<HeadquarterMedicine> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<HeadquarterMedicine> { Items = [], TotalItems = 0 };
        }
        return new TableData<HeadquarterMedicine>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadAsync();
        await table.ReloadServerData();
    }

    private void ReturnAction()
    {
        NavigationManager.NavigateTo("/headquarters");
    }

    private async Task ShowModalAsync()
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        var parameters = new DialogParameters
                {
                    { "Id", HeadquarterId }
                };

        var dialog = DialogService.Show<AddMedicine>(Localizer["AddMedicineToHeadquarter"], parameters, options);
        await dialog.Result;
        await LoadAsync();
        await table.ReloadServerData();
    }

    private void NoData()
    {
        NavigationManager.NavigateTo("/headquarters");
    }

    private async Task DeleteAsync(HeadquarterMedicine headquarterMedicine)
    {
        var parameters = new DialogParameters
        {
            { "Message", string.Format(Localizer["DeleteConfirm"], Localizer["Medicine"], headquarterMedicine.Medicine.Name) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };
        var dialog = DialogService.Show<ConfirmDialog>(Localizer["Confirmation"], parameters, options);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        var responseHttp = await Repository.DeleteAsync($"{baseUrlHeadquarterMedicine}/{headquarterMedicine.Id}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }
        await LoadAsync();
        await table.ReloadServerData();
        Snackbar.Add(Localizer["RecordDeletedOk"], Severity.Success);
    }
}