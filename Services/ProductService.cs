﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotShopApi.Models.Dto.Product;
using BotShopApi.Extensions.Product;
using BotShopCore;
using BotShopCore.Attributes;
using BotShopCore.Models;
using BotShopCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BotShopApi.Services {
  [Service]
  public class ProductService : RepositoryService<Product> {
    private MediaStorageService MediaStorageService { get; }

    public ProductService(
      ApplicationContext applicationContext,
      MediaStorageService mediaStorageService
    ) : base(applicationContext) =>
      MediaStorageService = mediaStorageService;

    public async Task<bool> CheckIsProductExistsAsync(int productId) =>
      await ByIdAsync(productId) != null;

    public async Task<bool> CheckIsValidNameAsync(string name, int accountId) {
      var commonProducts = await ByPatternManyAsync(product => product.AccountId == accountId && product.Name == name);

      return commonProducts.Count == 0;
    }

    public async Task<Product> CreateAsync(CreateProductDto productDto, int accountId) =>
      await AddEntityAsync(
        new Product {
          Name = productDto.Name,
          Description = productDto.Description,
          Category = productDto.Category,
          Price = productDto.Price,
          Type = productDto.Type,
          MediaUrl = await MediaStorageService.UploadMedia(productDto.Media, accountId),
          AccountId = accountId
        }
      );

    public async Task RemoveAsync(int id) {
      var product = await ByIdAsync(id);

      await MediaStorageService.RemoveMedia(product.MediaUrl);
      await RemoveByIdAsync(product.Id);
    }

    public async Task<Product> UpdateAsync(int id, int accountId, UpdateProductDto updateProductDto) {
      var productToUpdate = await ByIdAsync(id);

      productToUpdate.Name = updateProductDto.Name ?? productToUpdate.Name;
      productToUpdate.Description = updateProductDto.Description ?? productToUpdate.Description;
      productToUpdate.Price = updateProductDto.Price ?? productToUpdate.Price;
      productToUpdate.Category = updateProductDto.Category ?? productToUpdate.Category;
      productToUpdate.Type = updateProductDto.Type ?? productToUpdate.Type;

      if (updateProductDto.Media != null) {
        await MediaStorageService.RemoveMedia(productToUpdate.MediaUrl);
        productToUpdate.MediaUrl = await MediaStorageService.UploadMedia(updateProductDto.Media, accountId);
      }

      await SaveChangesAsync();

      return productToUpdate;
    }

    public async Task RemoveAllProductsAsync(int accountId) {
      var removedProducts = await RemoveManyByPatternAsync(p => p.AccountId == accountId);

      await MediaStorageService.RemoveManyMedia(
        removedProducts.Select(p => p.MediaUrl)
      );
    }

    public async Task<PaginationResult<Product>> PaginateAsync(
      int accountId, string category, string type, int page, int limit
    ) {
      return await PaginateByPatternAsync(
        product => product.AccountId == accountId && product.Category == category && product.Type == type,
        page,
        limit
      );
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(int accountId) {
      var products = await ByPatternManyAsync(
        product => product.AccountId == accountId
      );

      return products.GetCategories();
    }

    protected override DbSet<Product> Set => Context.Products;
  }
}
