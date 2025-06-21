using AutoMapper;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Ecom.Presentation.Controllers
{
    public class CartsController : BaseController
    {
        public CartsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("Get-By-Id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {

            try
            {
                var cart = await work.CartRepository.GetCartAsync(id);

                if (cart == null)
                {
                    return Ok(new Cart());
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [EnableRateLimiting("WritePolicy")]
        [HttpPost("Update-Or-Create")]
        public async Task<IActionResult> UpdateOrCreate(Cart cart)
        {
            try
            {
                var result = await work.CartRepository.UpdateOrCreateCartAsync(cart);
                
                if (result == null)
                {
                    return BadRequest(new ResponseAPI(400)); 
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [EnableRateLimiting("WritePolicy")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await work.CartRepository.DeleteCartAsync(id);
                
                if (!result)
                {
                    return BadRequest(new ResponseAPI(400));
                }

                return Ok(new ResponseAPI(200, "Cart deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
    }
}
