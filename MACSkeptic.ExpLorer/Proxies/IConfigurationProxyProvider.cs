using System;

namespace MACSkeptic.ExpLorer.Proxies
{
    public interface IConfigurationProxyProvider
    {
        object For(Type type, Configuration configuration);
    }
}