using Microsoft.AspNetCore.Mvc;
using MultiTenancy.Dtos;

namespace MultiTenancy.Controllers
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            return product is null ? NotFound() : Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Rate = productDTO.Rate,
            };

            var createdProduct = await _productService.CreateAsync(product);

            return Ok(createdProduct);
        }
    }
}
