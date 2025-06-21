using AutoMapper;
using Ecom.Application.DTOs.Category;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities.Product;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Ecom.Presentation.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work,IMapper mapper) : base(work,mapper)
        {
        }

        //Ok
        //Created
        //CreatedAt
        //CreatedAtAction
        //CreatedAtRoute
        //NoContent
        //BadRequest
        //Unauthorized
        //NotFound
        //Forbid
        //NoContent

        [EnableRateLimiting("ReadOnlyPolicy")]
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await work.CategoryRepository.GetAllAsync();

                if (categories is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [EnableRateLimiting("ReadOnlyPolicy")]
        [HttpGet("Get-By-Id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await work.CategoryRepository.GetByIdAsync(id);

                if(category is null)
                {
                    return BadRequest(new ResponseAPI(400, $"not found category id = {id}"));
                }
                return Ok(category);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add-Category")]
        public async Task<IActionResult> Add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);

                await work.CategoryRepository.AddAsync(category);

                return Ok(new ResponseAPI(200, "Item has been added"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update-Category")]
        public async Task<IActionResult> Update(UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                var newCategory = mapper.Map<Category>(updateCategoryDTO);

                await work.CategoryRepository.UpdateAsync(newCategory);

                return Ok(new ResponseAPI(200, "Item has been updated"));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete-Category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await work.CategoryRepository.DeleteAsync(id);

                return Ok(new ResponseAPI(200, "Item has been deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
