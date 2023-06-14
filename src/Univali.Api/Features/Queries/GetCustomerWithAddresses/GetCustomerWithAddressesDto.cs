using Univali.Api.Models;

namespace Univali.Api.Features.Queries.GetCustomerWithAddresses
{
    public class GetCustomerWithAddressesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
    }
}
