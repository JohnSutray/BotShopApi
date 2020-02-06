using System.Linq;
using System.Threading.Tasks;
using ImportShopApi.Contexts;
using ImportShopApi.Models.Product;
using ImportShopApi.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Services {
  public class ProductService {
    private ProductContext ProductContext { get; }
    private MediaStorageService MediaStorageService { get; }

    public ProductService(
      ProductContext productContext,
      MediaStorageService mediaStorageService
    ) {
      ProductContext = productContext;
      MediaStorageService = mediaStorageService;
    }

    public IQueryable<Product> Products => ProductContext.Products;

    public async Task<bool> IsValidName(string name, int ownerId)
      => !await Products.AnyAsync(p => p.Name == name && p.OwnerId == ownerId);

    public async Task CreateProduct(CreateProductDto productDto, int ownerId) {
      var product = new Product {
        Name = productDto.Name,
        Description = productDto.Description,
        Category = productDto.Category,
        Price = productDto.Price,
        Type = productDto.Type,
        OwnerId = ownerId,
        MediaUrl = await MediaStorageService.UploadMedia(productDto.Media, ownerId),
      };
      await ProductContext.AddAsync(product);
      await ProductContext.SaveChangesAsync();
    }

    public async Task<bool> RemoveProduct(int id, int ownerId) {
      var productToRemove = await FindProductById(id, ownerId);

      if (productToRemove == null) return false;

      await MediaStorageService.RemoveMedia(productToRemove.MediaUrl);
      ProductContext.Remove(productToRemove);
      await ProductContext.SaveChangesAsync();

      return true;
    }

    public async Task<bool> UpdateProduct(int id, int ownerId, UpdateProductDto updateProductDto) {
      var productToUpdate = await FindProductById(id, ownerId);

      if (productToUpdate == null) return false;

      productToUpdate.Name = updateProductDto.Name ?? productToUpdate.Name;
      productToUpdate.Description = updateProductDto.Description ?? productToUpdate.Description;
      productToUpdate.Price = updateProductDto.Price ?? productToUpdate.Price;
      productToUpdate.Category = updateProductDto.Category ?? productToUpdate.Category;
      productToUpdate.Type = updateProductDto.Type ?? productToUpdate.Type;

      if (updateProductDto.Media != null) {
        await MediaStorageService.RemoveMedia(productToUpdate.MediaUrl);
        productToUpdate.MediaUrl = await MediaStorageService.UploadMedia(updateProductDto.Media, ownerId);
      }

      await ProductContext.SaveChangesAsync();

      return true;
    }

    public async Task RemoveAllOwnerProducts(int ownerId) {
      var productsToDelete = Products.Where(p => p.OwnerId == ownerId);
      ProductContext.RemoveRange();
      await ProductContext.SaveChangesAsync();
      await MediaStorageService.RemoveOwnerMedia(productsToDelete.Select(p => p.MediaUrl));
    }

    private async Task<Product> FindProductById(int id, int ownerId)
      => await Products.FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == ownerId);
  }
}