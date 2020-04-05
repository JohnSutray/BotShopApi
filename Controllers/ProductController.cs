using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImportShopApi.Constants;
using ImportShopApi.Extensions;
using ImportShopApi.Extensions.Dto;
using ImportShopApi.Models.Dto.Product;
using ImportShopApi.Services;
using ImportShopCore.Extensions;
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
    public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto productDto) {
      if (!ModelState.IsValid) {
        return this.UnprocessableModelResult();
      }

      if (!await ProductService.CheckIsValidNameAsync(productDto.Name, AccountId)) {
        return this.AddModelError(Messages.ProductWithSelectedNameExists)
          .UnprocessableModelResult();
      }

      var createdProduct = await ProductService.CreateAsync(productDto, User.GetUserId());

      return Ok(createdProduct);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(string id, [FromForm] UpdateProductDto updateProductDto) {
      if (!await ProductService.CheckIsProductExistsAsync(id.ParseInt())) {
        return this.AddModelError(Messages.ProductWithSelectedIdNotExists).UnprocessableModelResult();
      }

      if (!ModelState.IsValid) {
        return this.UnprocessableModelResult();
      }

      var updatedProduct = await ProductService.UpdateAsync(id.ParseInt(), User.GetUserId(), updateProductDto);

      return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id) {
      if (!await ProductService.CheckIsProductExistsAsync(id.ParseInt())) {
        return this.AddModelError(Messages.ProductWithSelectedIdNotExists).UnprocessableModelResult();
      }

      await ProductService.RemoveAsync(id.ParseInt());

      return Ok();
    }

    [HttpGet("{category}/{type}/{page}/{limit}")]
    public async Task<PaginationResult<ProductDto>> GetProducts(
      string category, string type, int page, int limit
    ) {
      var productPage = await ProductService.PaginateAsync(User.GetUserId(), category, type, page, limit);

      return new PaginationResult<ProductDto> {
        Items = productPage.Items.Select(ProductDtoExtensions.ToDto).ToList(),
        Limit = productPage.Limit,
        Page = productPage.Page,
        TotalPages = productPage.TotalPages
      };
    }


    [HttpGet("category")]
    public async Task<IEnumerable<CategoryDto>> GetCategories() {
      var categories = await ProductService.GetCategoriesAsync(AccountId);

      return categories.Select(CategoryDtoExtensions.ToDto);
    }
  }
}