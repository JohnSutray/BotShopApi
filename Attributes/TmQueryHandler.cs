using System;

namespace ImportShopBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TmQueryHandler : BaseTmHandler
    {
        public TmQueryHandler(string regex = null): base(regex) {}
    }
}