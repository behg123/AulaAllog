using System.ComponentModel.DataAnnotations;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Models;

public abstract class CustomerForManipulationDto
{
    [Required(ErrorMessage = "You need to fill the Name camp")]
    [MaxLength(100, ErrorMessage = "You exceeded the total character count for the name, Try Again")]
    public string Name {get; set;} = string.Empty;
    [Required(ErrorMessage = "You need to fill the Cpf camp")]
    //[StringLength(11, MinimumLength = 11, ErrorMessage = "The Cpf is invalid, it mustc have 11 characters")]
    [CpfMustBeValid(ErrorMessage = "The provided {0} needs to be a valid number")]
    public string Cpf {get; set;} = string.Empty;
}
