using AutoMapper;
using Ecom.Application.DTOs.Product;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Shared;
using Ecom.Presentation.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Presentation.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll([FromQuery]ProductParams productParams)
        {
            try
            {
                var products = await work.ProductRepository
                    .GetAllAsync(productParams);

                if (products is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }

                return Ok(new Pagination<ProductDTO>(productParams.PageNumber, productParams.PageSize, productParams.TotalCount, products));
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
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }


        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await work.ProductRepository
                    .GetByIdAsync(id, p => p.Photos, product => product.Category);

                if (product is null)
                {
                    return BadRequest(new ResponseAPI(400, $"Product not found"));
                }

                await work.ProductRepository.DeleteAsync(product);

                return Ok(new ResponseAPI(200, "Product deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
    }
}


