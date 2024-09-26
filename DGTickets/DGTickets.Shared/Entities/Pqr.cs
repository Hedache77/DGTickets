using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGTickets.Shared.Entities;

public class Pqr
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [MaxLength(500)]
    [Required]

    public string Description { get; set; }
}
