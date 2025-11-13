using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController: ControllerBase
{
  [HttpGet]
  public ActionResult<CommonResult<string>> GetProduct()
  {
    return CommonResult<string>.Fail("Result not found");
  }
}