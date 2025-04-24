using AutoMapper;
using Ecom.Application.DTOs;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities.Product;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("get-all")]
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

        [HttpGet("get-by-id/{id}")]
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

        [HttpPost("add-category")]
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

        [HttpPut("update-category")]
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

        [HttpDelete("delete-category/{id}")]
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
