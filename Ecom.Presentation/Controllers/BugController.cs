using AutoMapper;
using Ecom.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Presentation.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("Not-Found")]
        public async Task<IActionResult> GetNotFound()
        {
            var category = await work.CategoryRepository.GetByIdAsync(1000);

            if (category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(category);
        }

        [HttpGet("Server-Error")]
        public async Task<IActionResult> GetServerError()
        {
            var category = await work.CategoryRepository.GetByIdAsync(1000);
           
            // Simulate a server error
            category.Name = "";

            return Ok(category);
        }

        [HttpGet("Bad-Request/{id}")]
        public async Task<IActionResult> GetBadRequest(int id)
        {
            return Ok();
        }

        [HttpGet("Bad-Request")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
