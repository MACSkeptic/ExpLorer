using System;

namespace MACSkeptic.ExpLorer
{
    public interface IConfigurationProxyProvider
    {
        object For(Type type, Configuration configuration);
    }
}