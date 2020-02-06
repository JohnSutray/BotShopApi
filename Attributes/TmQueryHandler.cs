using System;

namespace ImportShopApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TmQueryHandler : BaseTmHandler
    {
        public TmQueryHandler(string regex = null): base(regex) {}
    }
}