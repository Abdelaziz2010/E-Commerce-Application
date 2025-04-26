using AutoMapper;
using Ecom.Application.DTOs;
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

        [HttpGet("get-all")]
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
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
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
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add-product")]
        public async Task<IActionResult> Create([FromBody]ProductDTO productDTO)
        {
            try
            {
                var product = mapper.Map<Product>(productDTO);

                await work.ProductRepository.AddAsync(product);

                return Ok(new ResponseAPI(200, "Product created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> Update([FromBody] ProductDTO productDTO)
        {
            try
            {
                var product = mapper.Map<Product>(productDTO);
                                
                if (product is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }

                await work.ProductRepository.UpdateAsync(product);

                return Ok(new ResponseAPI(200, "Product updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
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


