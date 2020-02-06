using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ImportShopApi.Attributes;
using ImportShopApi.Models.Telegram;

namespace ImportShopApi.Extensions {
  public static class MethodInfoExtensions {
    private static TAttribute GetTmHandlerAttribute<TAttribute>(this MethodInfo methodBase)
      where TAttribute : BaseTmHandler => methodBase
      .GetCustomAttributes()
      .First(a => a is TAttribute) as TAttribute;

    private static TmHandler<TContext> ToTmHandler<TContext>(this MethodInfo methodBase, object controller)
      where TContext : TmContext =>
      context => methodBase.Invoke(controller, new object[] {context}) as Task<bool>;

    public static TmHandlerContainer<TContext> ToTmContainer
      <TContext>(this MethodInfo methodInfo, object controller)
      where TContext : TmContext
      => new TmHandlerContainer<TContext>(
        methodInfo.GetTmHandlerAttribute<BaseTmHandler>().HandlerRegex,
        methodInfo.ToTmHandler<TContext>(controller)
      );

    public static bool HasAttribute<TAttribute>(this MethodInfo methodInfo) where TAttribute : Attribute
      => methodInfo.GetCustomAttributes().Any(a => a is TAttribute);
  }
}