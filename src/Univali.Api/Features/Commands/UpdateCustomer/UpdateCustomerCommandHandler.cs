using AutoMapper;
using MediatR;
using Univali.Api.Features.Commands.UpdateCustomer;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Features.CommandHandlers.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<UpdateCustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(request.Id);

            if (customerFromDatabase == null) return null!;
            
            _mapper.Map(request, customerFromDatabase);

            await _customerRepository.SaveChangesAsync();

            var updatedCustomerDto = _mapper.Map<UpdateCustomerDto>(customerFromDatabase);

            return updatedCustomerDto;
        }
    }
}
