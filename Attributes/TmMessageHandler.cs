using System;

namespace ImportShopApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TmMessageHandler : BaseTmHandler
    {
        public TmMessageHandler(string regex = null): base(regex) {}
    }
}