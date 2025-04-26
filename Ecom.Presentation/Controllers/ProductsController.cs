using AutoMapper;
using Ecom.Application.DTOs.Product;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities.Product;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Presentation.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await work.ProductRepository
                    .GetAllAsync(p => p.Category, p => p.Photos);

                var result = mapper.Map<List<ProductDTO>>(products);

                if (result is null)
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
        [HttpGet("Get-By-Id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await work.ProductRepository
                    .GetByIdAsync(id, p => p.Category, p => p.Photos);

                var result = mapper.Map<ProductDTO>(product);

                if(result is null)
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


        [HttpPost("Add-Product")]
        public async Task<IActionResult> Add(AddProductDTO productDTO)
        {
            try
            {
                await work.ProductRepository.AddAsync(productDTO);

                return Ok(new ResponseAPI(200, "Product created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await work.ProductRepository.UpdateAsync(updateProductDTO);

                return Ok(new ResponseAPI(200, "Product updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await work.ProductRepository.GetByIdAsync(id);
                if (product is null)
                {
                    return BadRequest(new ResponseAPI(400, $"Product with id {id} not found"));
                }
                await work.ProductRepository.DeleteAsync(id);

                return Ok(new ResponseAPI(200, "Product deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


