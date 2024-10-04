using CurrieTechnologies.Razor.SweetAlert2;
using DGTickets.Frontend.Repositories;
using DGTickets.Shared.Entities;
using DGTickets.Shared.Resources;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DGTickets.Frontend.Pages.MedicinesStock;

public partial class MedicineStockForm
{
    private EditContext editContext = null!;
    private string? imageUrl;
    private string? shapeImageMessage;

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    [EditorRequired, Parameter] public MedicineStock MedicineStock { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;

    protected override void OnInitialized()
    {
        editContext = new(MedicineStock);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!string.IsNullOrEmpty(MedicineStock.Image))
        {
            imageUrl = MedicineStock.Image;
            MedicineStock.Image = null;
        }
        shapeImageMessage = MedicineStock.IsImageSquare ? Localizer["ImageIsSquare"] : Localizer["ImageIsRectangular"];
    }

    private void OnToggledChanged(bool toggled)
    {
        MedicineStock.IsImageSquare = toggled;
        shapeImageMessage = MedicineStock.IsImageSquare ? Localizer["ImageIsSquare"] : Localizer["ImageIsRectangular"];
    }

    private void ImageSelected(string imagenBase64)
    {
        MedicineStock.Image = imagenBase64;
        imageUrl = null;
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
}