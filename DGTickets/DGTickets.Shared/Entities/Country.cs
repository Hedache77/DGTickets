using DGTickets.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGTickets.Shared.Entities;

public class Country
{
    public int Id { get; set; }

    [Display(Name = "Country", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public ICollection<State>? States { get; set; }

    public int StatesCount => States == null ? 0 : States.Count;
}