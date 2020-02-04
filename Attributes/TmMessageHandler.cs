using System;

namespace ImportShopBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TmMessageHandler : BaseTmHandler
    {
        public TmMessageHandler(string regex = null): base(regex) {}
    }
}