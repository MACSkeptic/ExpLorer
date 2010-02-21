using Castle.DynamicProxy;
using MACSkeptic.ExpLorer.Parsers;
using MACSkeptic.ExpLorer.Proxies.Interceptors;

namespace MACSkeptic.ExpLorer.Proxies
{
    public class ConfigurationProxy
    {
        public static T For<T>(Configuration configuration)
            where T : class
        {
            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(
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