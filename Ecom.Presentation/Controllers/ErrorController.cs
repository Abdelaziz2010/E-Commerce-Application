using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Presentation.Controllers
{
    [Route("errors/{statuscode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int statuscode)
        {
            return new ObjectResult(new ResponseAPI(statuscode));
        }
    }
}
