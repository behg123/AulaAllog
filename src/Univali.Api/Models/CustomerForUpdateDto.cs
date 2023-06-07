using System.ComponentModel.DataAnnotations;

namespace Univali.Api.Models;

public class CustomerForUpdateDto: CustomerForManipulationDto
{
    [Required(ErrorMessage = "You need to fill out the ID")]
    public int Id { get; set; }
}