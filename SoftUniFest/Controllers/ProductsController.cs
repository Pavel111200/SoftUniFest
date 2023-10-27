using Data.Dtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SoftUniFest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Create(Guid companyId, CreateProductDto product)
        {
            bool isSaved;

            isSaved = await _productService.AddProduct(companyId, product);

            if (isSaved)
            {
                return Ok();
            }
            return BadRequest("Error occured while creating the product");
        }

        [HttpGet]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> GetAll(Guid companyId)
        {
            var result = await _productService.GetAllProducts(companyId);

            return Ok(result);
        }

        [HttpGet("{productId}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> GetOne(Guid productId)
        {
            var result = await _productService.GetProductById(productId);

            return Ok(result);
        }
    }
}
