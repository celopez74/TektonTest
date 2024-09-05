using Microsoft.AspNetCore.Mvc;
using Tekton.Products.Api.Controllers.V1;

namespace Tekton.Products.Api.Controllers;

public class ValidatorController : ControllerBase
{
    private readonly string _validationText;

    public ValidatorController(IConfiguration configuration)
    {
        _validationText = configuration.GetValue<string>("TektonApisValidationText");
    }
    [HttpGet("/tektonapisverifydomain")]
    public async Task<IActionResult> GetTektonApis()
    {
        return Ok(_validationText);       
    }
}