using Castle.Core.Interceptor;
using Castle.DynamicProxy;

namespace MACSkeptic.ExpLorer.Proxies.Interceptors
{
    public class ConfigurationInterceptor : IInterceptor
    {
        private readonly Configuration _configuration;

        public ConfigurationInterceptor(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;

            if (!method.Name.Contains("get_"))
            {
                invocation.Proceed();
                return;
            }

            var name = method.Name.Replace("get_", string.Empty);
            var type = method.ReturnType;

            if (type.IsInterface)
            {
                invocation.ReturnValue = new ProxyGenerator().CreateInterfaceProxyWithoutTarget(
                    type, new ConfigurationInterceptor(_configuration.Get(name)));
                return;
            }

            if (typeof (string) == type)
            {
                invocation.ReturnValue = _configuration.Get(name).Value;
                return;
            }

            invocation.Proceed();
            return;
        }
    }
}