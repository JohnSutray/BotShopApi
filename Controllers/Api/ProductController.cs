using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImportShopBot.Extensions;
using ImportShopBot.Extensions.Enumerable;
using ImportShopBot.Models;
using ImportShopBot.Models.Product;
using ImportShopBot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopBot.Controllers.Api
{
    public static class ProductQueryableExtensions
    {
        public static IEnumerable<Category> GetCategories(this IQueryable<Product> products, int userId) =>
            products.Where(p => p.OwnerId == userId)
                .AsEnumerable()
                .GetGroups(p => p.Category)
                .Select(products.GetCategoryWithTypes);

        private static Category GetCategoryWithTypes(this IQueryable<Product> products, string category) =>
            new Category
            {
                Name = category,
                Types = products.Where(p => p.Category == category)
                    .AsEnumerable()
                    .GetGroups(p => p.Type)
            };
    }

    [Authorize]
    [ApiController]
    [Route("product")]
    public class ProductController : Controller
    {
        private ProductService ProductService { get; }

        public ProductController(ProductService productService) => ProductService = productService;

        [HttpPut]
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
        public PaginateResult<Product> GetProducts(string category, string type, int page, int limit) =>
            ProductService
                .Products
                .Where(p => p.Category == category && p.Type == type && p.OwnerId == User.GetUserId())
                .AsEnumerable()
                .Paginate(page, limit);

        [HttpGet("category")]
        public IEnumerable<Category> GetCategories() => ProductService.Products.GetCategories(User.GetUserId());
    }
}