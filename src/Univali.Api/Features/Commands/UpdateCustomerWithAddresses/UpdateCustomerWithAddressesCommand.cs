using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Univali.Api.Entities;
using Univali.Api.Features.Commands.CreateCustomer.UpdateCustomerWithAddresses;

namespace Univali.Api.Features.Commands.UpdateCustomerWithAddresses
{
    public class UpdateCustomerWithAddressesCommand : IRequest<UpdateCustomerWithAddressesDto>
    {
        [Required(ErrorMessage = "You need to fill out the ID")]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}