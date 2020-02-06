using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ImportShopApi.Extensions {
  public static class TypeExtensions {
    public static IEnumerable<MethodInfo> GetMethodsWithAttribute<TAttribute>(this Type type)
      where TAttribute : Attribute
      => type.GetMethods().Where(m => m.HasAttribute<TAttribute>());
  }
}