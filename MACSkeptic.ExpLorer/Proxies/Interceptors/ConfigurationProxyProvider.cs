using System;
using Castle.DynamicProxy;

namespace MACSkeptic.ExpLorer.Proxies.Interceptors
{
    public class ConfigurationProxyProvider : IConfigurationProxyProvider
    {
        private readonly ProxyGenerator _generator;

        public ConfigurationProxyProvider()
        {
            _generator = new ProxyGenerator();
        }

        public object For(Type type, Configuration configuration)
        {
            return _generator.CreateInterfaceProxyWithoutTarget(
                type,
                new ConfigurationInterceptor(configuration));
        }
    }
}