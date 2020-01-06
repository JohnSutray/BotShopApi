using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ImportShopBot.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static IEnumerable<string> GetErrors(this ModelStateDictionary modelStateDictionary) => modelStateDictionary
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
    }
}