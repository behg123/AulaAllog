using MediatR;
using System.ComponentModel.DataAnnotations;
using Univali.Api.Models;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<UpdateCustomerDto>
    {
        [Required(ErrorMessage = "You need to fill out the ID")]
        public int Id { get; set; }
    
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }
}
