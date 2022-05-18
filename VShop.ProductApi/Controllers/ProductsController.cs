using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Services;

namespace VShop.ProductApi.Controllers
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var produtosDto = await _productService.GetProducts();
            if(produtosDto == null)
            {
                return NotFound("Products not found");
            }
            return Ok(produtosDto);
        }
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var produtoDto = await _productService.GetProductById(id);
            if(produtoDto == null)
            {
                return NotFound("Product not found");
            }
            return Ok(produtoDto);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest("Data Invalid");

            await _productService.AddProduct(productDto);

            return new CreatedAtRouteResult("GetProduct",
                new { id = productDto.Id }, productDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>>Delete(int id)
        {
            var produtoDto = await _productService.GetProductById(id);

            if(produtoDto == null)
            {
                return NotFound("Product not found");
            }

            await _productService.RemoveProduct(id);

            return Ok(produtoDto);
        }
    }
}
