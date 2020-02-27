using System.Collections.Generic;
using System.Threading.Tasks;
using ImportShopCore.Models.Product;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Authentication;
using ImportShopApi.Models;
using ImportShopApi.Services;
using ImportShopCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImportShopApi.Controllers {
  [Authorize]
  [ApiController]
  [Route("product")]
  public class ProductController : Controller {
    private int AccountId => User.GetUserId();
    private ProductService ProductService { get; }

    public ProductController(ProductService productService) => ProductService = productService;

    [HttpPut]
    public async Task<ActionResult> Create([FromForm] CreateProductDto productDto) {
      if (!ModelState.IsValid) {
        return this.UnprocessableModelResult();
      }

      if (!await ProductService.CheckIsValidNameAsync(productDto.Name, AccountId)) {
        return this.AddModelError("Продукт с таким именем уже существует").UnprocessableModelResult();
      }

      await ProductService.CreateAsync(productDto, User.GetUserId());

      return Ok();
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto) {
      if (!await ProductService.CheckIsProductExistsAsync(id)) {
        return this.AddModelError("Указанного продукта не существует").UnprocessableModelResult();
      }

      if (!ModelState.IsValid) {
        return this.UnprocessableModelResult();
      }

      await ProductService.UpdateAsync(id, User.GetUserId(), updateProductDto);

      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) {
      if (!await ProductService.CheckIsProductExistsAsync(id)) {
        return this.AddModelError("Указанного продукта не существует").UnprocessableModelResult();
      }

      await ProductService.RemoveProductAsync(id);

      return Ok();
    }

    [HttpGet("{category}/{type}/{page}/{limit}")]
    public async Task<PaginateResult<Product>> GetProducts(
      string category, string type, int page, int limit
    ) => await ProductService.PaginateAsync(User.GetUserId(), category, type, page, limit);


    [HttpGet("category")]
    public async Task<IEnumerable<Category>> GetCategories() =>
      await ProductService.GetCategoriesAsync(AccountId);
  }
}