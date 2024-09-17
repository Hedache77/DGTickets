using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DGTickets.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicineStockController : GenericController<MedicineStock>
{
    public MedicineStockController(IGenericUnitOfWork<MedicineStock> unitOfWork) : base(unitOfWork)
    {
    }
}