using System.Collections.Generic;
using System.Threading.Tasks;
using ImportShopBot.Extensions;
using ImportShopBot.Models.Product;
using ImportShopBot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopBot.Controllers
{
    [Authorize]
    [ApiController]
    [Route("product")]
    public class ProductController : Controller
    {
        private ProductService ProductService { get; }

        public ProductController(ProductService productService) => ProductService = productService;

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await ProductService.IsValidName(productDto.Name, User.GetUserId()))
                return this.AddModelError("Продукт с таким именем уже существует").UnprocessableModel();

            await ProductService.CreateProduct(productDto, User.GetUserId());
            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            if (!await ProductService.UpdateProduct(id, User.GetUserId(), updateProductDto))
                return this.AddModelError("Указанного продукта не существует").UnprocessableModel();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) => await ProductService.RemoveProduct(id, User.GetUserId())
            ? Ok()
            : this.AddModelError("Указанного продукта не существует").UnprocessableModel();

        [HttpGet("{category}/{type}/{page}/{limit}")]
        public async Task<IEnumerable<Product>> GetProducts(string category, string type, int page, int limit)
            => await ProductService.Products
                .PaginateAndFilterAsync(page, limit, p => p.Category == category && p.Type == type);
    }
}