using Courseproject.Common.Dtos.Address;

//64
namespace Courseproject.Common.Interfaces;

public interface IAddressService
{
    //Create addresses
    //returns id
    //addressCreate dto
    Task<int> CreateAddressAsync(AddressCreate addressCreate);
    Task UpdateAddressAsync(AddressUpdate addressUpdate);
    Task DeleteAddressAsync(AddressDelete addressDelete);
    Task<AddressGet> GetAddressAsync(int id);
    Task<List<AddressGet>> GetAddressesAsync();
}
