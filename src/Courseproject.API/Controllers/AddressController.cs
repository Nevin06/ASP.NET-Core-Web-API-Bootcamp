using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courseproject.API.Controllers;
//64
[ApiController]
[Authorize] //93
[Route("[controller]")]
//route of this will be Address only, 'controller' reserve key word cut out
public class AddressController : ControllerBase
{
    private IAddressService AddressService { get; }

    //inject service
    public AddressController(IAddressService addressService)
	{
        AddressService = addressService;
    }

    //endpoint
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateAddress(AddressCreate addressCreate)
    {
        var id = await AddressService.CreateAddressAsync(addressCreate);
        return Ok(id);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateAddress(AddressUpdate addressUpdate)
    {
        await AddressService.UpdateAddressAsync(addressUpdate);
        return Ok();
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<IActionResult> DeleteAddress(AddressDelete addressDelete)
    {
        await AddressService.DeleteAddressAsync(addressDelete);
        return Ok();
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetAddress(int id)
    {
        var address = await AddressService.GetAddressAsync(id);
        return Ok(address);
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> GetAddresses()
    {
        //71
        //throw new Exception("Test");
        //95
        var whitelist = new List<string>() { "thomasnevink@gmail.com"}; 
        var email = HttpContext.User.Claims.First(c => c.Type == "preferred_username").Value;
        if (!whitelist.Contains(email))
            return new ForbidResult(); //to tell user/client that he has access to this endpoint  

        var addresses = await AddressService.GetAddressesAsync();
        return Ok(addresses);
    }
}
