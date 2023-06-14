using System.ComponentModel.DataAnnotations;
using MediatR;
using Univali.Api.Entities;
using Univali.Api.Features.Commands.CreateCustomer.WithAddresses;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerWithAddressesCommand : IRequest<CreateCustomerWithAddressesDto>
{
    [Required(ErrorMessage = "You should fill out a Name")]
    [MaxLength(100, ErrorMessage = "The name shouldn't have more than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should fill out a Cpf")]
    [CpfMustBeValid(ErrorMessage = "The provided {0} should be valid number.")]
    public string Cpf { get; set; } = string.Empty;
    public ICollection<Address> Addresses { get; set; } = new List<Address>();

}

