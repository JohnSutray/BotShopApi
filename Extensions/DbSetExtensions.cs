using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImportShopApi.Extensions
{
  public static class DbSetExtensions
  {
    public static async Task<TModel> AddAndSaveAsync<TModel>(this DbContext context, TModel model)
    {
      context.Add(model);
      await context.SaveChangesAsync();

      return model;
    }
  }
}