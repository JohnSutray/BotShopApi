using System;
using System.Text.RegularExpressions;

namespace ImportShopApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BaseTmHandler : Attribute
    {
        public Regex HandlerRegex { get; }

        protected BaseTmHandler(string regex = null) => HandlerRegex = regex != null
            ? new Regex(regex)
            : new Regex(".*");
    }
}