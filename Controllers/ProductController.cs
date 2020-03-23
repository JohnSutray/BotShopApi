using System.Collections.Generic;
using System.Threading.Tasks;
using ImportShopApi.Constants;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Authentication;
using ImportShopApi.Services;
using ImportShopCore.Models;
using ImportShopCore.Models.Dto;
using ImportShopCore.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ImportShopApi.Controllers {
  [Authorize]
  [ApiController]
  [Route("product")]
  public class ProductController : Controller {
    private int AccountId => User.GetUserId();
    private ProductService ProductService { get; }

    public ProductController(ProductService productService) => ProductService = productService;

    [HttpPut]
    [SwaggerOperation(OperationId = "createProduct")]
    public async Task<ActionResult> Create([FromForm] CreateProductDto productDto) {
      if (!ModelState.IsValid) {
        return this.UnprocessableModelResult();
      }

      if (!await ProductService.CheckIsValidNameAsync(productDto.Name, AccountId)) {
        return this.AddModelError(Messages.ProductWithSelectedNameExists)
          .UnprocessableModelResult();
      }

      await ProductService.CreateAsync(productDto, User.GetUserId());

      return Ok();
    }

    [HttpPost("{id}")]
    [SwaggerOperation(OperationId = "updateProduct")]
    public async Task<ActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto) {
      if (!await ProductService.CheckIsProductExistsAsync(id)) {
        return this.AddModelError(Messages.ProductWithSelectedIdNotExists).UnprocessableModelResult();
      }

      if (!ModelState.IsValid) {
        return this.UnprocessableModelResult();
      }

      await ProductService.UpdateAsync(id, User.GetUserId(), updateProductDto);

      return Ok();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(OperationId = "removeProduct")]
    public async Task<ActionResult> Delete(int id) {
      if (!await ProductService.CheckIsProductExistsAsync(id)) {
        return this.AddModelError(Messages.ProductWithSelectedIdNotExists).UnprocessableModelResult();
      }

      await ProductService.RemoveProductAsync(id);

      return Ok();
    }

    [HttpGet("{category}/{type}/{page}/{limit}")]
    [SwaggerOperation(OperationId = "getProducts")]
    public async Task<PaginateResult<Product>> GetProducts(
      string category, string type, int page, int limit
    ) => await ProductService.PaginateAsync(User.GetUserId(), category, type, page, limit);


    [HttpGet("category")]
    [SwaggerOperation(OperationId = "getProductCategories")]
    public async Task<IEnumerable<Category>> GetCategories() =>
      await ProductService.GetCategoriesAsync(AccountId);
  }
}