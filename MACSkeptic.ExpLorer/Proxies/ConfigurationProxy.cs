using System;
using Castle.DynamicProxy;
using MACSkeptic.ExpLorer.Parsers;
using MACSkeptic.ExpLorer.Proxies.Interceptors;

namespace MACSkeptic.ExpLorer.Proxies
{
    [Obsolete("As from v1.1.0 you should use 'MACSkeptic.ExpLorer.Sage' to create proxies instead")]
    public class ConfigurationProxy
    {
        public static T For<T>(Configuration configuration)
            where T : class
        {
            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(
                new ConfigurationInterceptor(configuration));
        }

        public static object For(Type type, Configuration configuration)
        {
            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(type,
                new ConfigurationInterceptor(configuration));
        }

        public static T For<T>(IConfigurationParser parser)
            where T : class
        {
            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(
                new ConfigurationInterceptor(parser.LoadFromCurrentAssembly()));
        }
    }
}