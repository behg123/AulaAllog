using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Features.Queries.GetCustomerByCpf
{
    public class GetCustomerByCpfQueryHandler : IRequestHandler<GetCustomerByCpfQuery, GetCustomerByCpfDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByCpfQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<GetCustomerByCpfDto> Handle(GetCustomerByCpfQuery request, CancellationToken cancellationToken)
        {
            var customerFromDatabase = await _customerRepository.FindCustomerByCpf(request.Cpf);
            if (customerFromDatabase == null)
                return null!;

            var customerToReturn = _mapper.Map<GetCustomerByCpfDto>(customerFromDatabase);
            return customerToReturn;
        }
    }
}
