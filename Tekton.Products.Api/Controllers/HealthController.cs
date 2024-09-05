using Microsoft.AspNetCore.Mvc;
using Tekton.Products.Api.Controllers.V1;

namespace Tekton.Products.Api.Controllers;

public class HealthController : TektonController
{
    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        return Ok("OK");
    }
}