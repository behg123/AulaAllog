using AutoMapper;
using Univali.Api.Repositores;

namespace Univali.Api.Features.Customers.Queries.GetCustomerDetail;

public class GetCustomerDetailQueryHandler: IGetCustomerDetailQueryHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerDetailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerDetailDto?> Handle(GetCustomerDetailQuery request)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(request.Id);
        return _mapper.Map<GetCustomerDetailDto>(customerFromDatabase);
    }
}