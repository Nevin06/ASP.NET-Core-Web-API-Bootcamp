using AutoMapper;
using Courseproject.Business.Exceptions;
using Courseproject.Business.Validation;
using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using FluentValidation;
//65

namespace Courseproject.Business.Services;

public class AddressService : IAddressService
{
    private IMapper Mapper { get; }
    private IGenericRepository<Address> AddressRepository { get; }
    private AddressCreateValidator AddressCreateValidator { get; }
    private AddressUpdateValidator AddressUpdateValidator { get; }

    public AddressService(IMapper mapper, IGenericRepository<Address> addressRepository,
        AddressCreateValidator addressCreateValidator, AddressUpdateValidator addressUpdateValidator)
    {
        Mapper = mapper;
        AddressRepository = addressRepository;
        //72
        AddressCreateValidator = addressCreateValidator;
        AddressUpdateValidator = addressUpdateValidator;
    }

    public async Task<int> CreateAddressAsync(AddressCreate addressCreate)
    {
        //72
        await AddressCreateValidator.ValidateAndThrowAsync(addressCreate);

        var entity = Mapper.Map<Address>(addressCreate);
        await AddressRepository.InsertAsync(entity);
        await AddressRepository.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteAddressAsync(AddressDelete addressDelete)
    {
        var entity = await AddressRepository.GetByIdAsync(addressDelete.Id,
            (address) => address.Employees);  //73
        //73
        if (entity == null)
            throw new AddressNotFoundException(addressDelete.Id);
        //Don't want to delete addresses if we have an employee that uses this address
        if (entity.Employees.Count > 0)
            throw new DependentEmployeesExistException(entity.Employees);

        AddressRepository.Delete(entity);
        await AddressRepository.SaveChangesAsync();
    }

    public async Task<AddressGet> GetAddressAsync(int id)
    {
        var entity = await AddressRepository.GetByIdAsync(id);
        //73
        if (entity == null)
            throw new AddressNotFoundException(id);

        return Mapper.Map<AddressGet>(entity);
    }

    public async Task<List<AddressGet>> GetAddressesAsync()
    {
        var entities = await AddressRepository.GetAsync(null, null);
        return Mapper.Map<List<AddressGet>>(entities);
    }

    public async Task UpdateAddressAsync(AddressUpdate addressUpdate)
    {
        //72
        await AddressUpdateValidator.ValidateAndThrowAsync(addressUpdate);

        //73
        var existingAddress = await AddressRepository.GetByIdAsync(addressUpdate.Id); //db roundtrip
        if (existingAddress == null)
            throw new AddressNotFoundException(addressUpdate.Id);

        //var existingEntity = await AddressRepository.GetByIdAsync(addressUpdate.Id);
        var entity = Mapper.Map<Address>(addressUpdate);
        AddressRepository.Update(entity);
        await AddressRepository.SaveChangesAsync();
    }
}
