using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Univali.Api.Entities;
using Univali.Api.Features.Commands.CreateCustomer.UpdateCustomerWithAddresses;
using Univali.Api.Features.Commands.UpdateCustomer;
using Univali.Api.Features.Commands.UpdateCustomerWithAddresses;
using Univali.Api.Models;
using Univali.Api.Repositories;

namespace Univali.Api.Features.CommandHandlers.UpdateCustomerWithAddresses;

public class UpdateCustomerWithAddressesCommandHandler : IRequestHandler<UpdateCustomerWithAddressesCommand, UpdateCustomerWithAddressesDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UpdateCustomerWithAddressesCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }


    public async Task<UpdateCustomerWithAddressesDto> Handle(UpdateCustomerWithAddressesCommand request, CancellationToken cancellationToken)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesByIdAsync(request.Id);

        if (customerFromDatabase == null)
            return null!;

        _mapper.Map(request, customerFromDatabase);


        await _customerRepository.SaveChangesAsync();

        var updatedCustomerDto = _mapper.Map<UpdateCustomerWithAddressesDto>(customerFromDatabase);

        return updatedCustomerDto;
    }
}


