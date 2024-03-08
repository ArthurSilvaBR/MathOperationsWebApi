using MathOperations.Models;
using MathOperations.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MathOperations.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MathOperations : ControllerBase
  {
    private readonly IMathExpressionEvualationService _mathExpressionEvualationService;

    public MathOperations(IMathExpressionEvualationService mathExpression)
    {
      _mathExpressionEvualationService = mathExpression;
    }

    [HttpPost(Name = "MathOperations")]
    public object MathOperation(MathOperation operation)
    {
      return _mathExpressionEvualationService.Evaluate(operation.Expression ?? string.Empty);
    }
  }
}
