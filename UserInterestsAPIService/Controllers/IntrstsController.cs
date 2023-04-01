using UserInterestsAPIService.Models;
using UserInterestsAPIService.Services;
using Microsoft.AspNetCore.Mvc;

namespace UserInterestsAPIService.Controllers;

[ApiController]
[Route("[controller]")]
public class IntrstsController : ControllerBase
    {
        public readonly I_IntrstsCosmosService _intrstCosmosService;
        public IntrstsController(I_IntrstsCosmosService intrstCosmosService)
        {
        _intrstCosmosService = intrstCosmosService;
        }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var sqlCosmosQuery = "Select * from c";
        var result = await _intrstCosmosService.Get(sqlCosmosQuery);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Intrsts intrstNew)
    {
        intrstNew.Id = Guid.NewGuid().ToString();
        var result = await _intrstCosmosService.AddAsync(intrstNew);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Intrsts intrstToUpdate)
    {
        var result = await _intrstCosmosService.Update(intrstToUpdate);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id, string intrsts)
    {
        await _intrstCosmosService.Delete(id, intrsts);
        return Ok();
    }

}

