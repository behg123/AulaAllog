using MediatR;using Univali.Api.Models;

namespace Univali.Api.Features.Queries.GetCustomerByCpf
{
    public class GetCustomerByCpfQuery : IRequest<GetCustomerByCpfDto>
    {
        public string Cpf { get; set; } = string.Empty;
    }
}
