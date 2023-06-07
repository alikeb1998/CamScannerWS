using CamScanner.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace CamScanner.Controllers;

[Produces("application/json")]
[Route("V1/[controller]")]
public class ProcessController : Controller
{
 
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public ProcessController( ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    // [HttpPost(nameof(UploadImage))]
    // public async Task<IActionResult> UploadImage([FromBody] Image input)
    // {
    //     var res = await _imageService.Upload(input);
    //     return Ok();
    // }

    [HttpPost(nameof(AddToRabbit))]
    public async Task<IActionResult> AddToRabbit([FromBody]Image model)
    {
        // var res = new Image() { Data = "dsgdg", Id = 14, ConnectionId = Guid.NewGuid().ToString() };
        
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:upload"));
        await endpoint.Send(model);
       
        return Ok();
    }
}