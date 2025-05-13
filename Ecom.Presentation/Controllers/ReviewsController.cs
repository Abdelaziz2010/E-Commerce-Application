using AutoMapper;
using Ecom.Application.DTOs.Reviews;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.Presentation.Controllers
{
    [Authorize]
    public class ReviewsController : BaseController
    {
        public ReviewsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {

        }

        [HttpGet("Get-All-Reviews-For-Product/{productId}")]
        public async Task<IActionResult> GetAllReviewsForProduct(int productId)
        {
            var reviews = await work.ReviewRepository.GetAllReviewsForProductAsync(productId);

            if(reviews is null)
            {
                return NotFound();
            }

            return Ok(reviews);
        }

        [HttpPost("Add-Review")]
        public async Task<IActionResult> AddReview(ReviewDTO reviewDTO)
        {
            if (reviewDTO is null)
            {
                return BadRequest(new ResponseAPI(400));
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email is null)
            {
                return BadRequest(new ResponseAPI(400));
            }

            var result = await work.ReviewRepository.AddReviewAsync(reviewDTO, email);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }

        [HttpDelete("Delete-Review/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var result = await work.ReviewRepository.DeleteReviewAsync(reviewId);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }
    }
}
