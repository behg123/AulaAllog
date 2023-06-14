using System.ComponentModel.DataAnnotations;
using Univali.Api.ValidationAttributes;

namespace Univali.Api.Features.Commands.UpdateCustomer
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }
}
