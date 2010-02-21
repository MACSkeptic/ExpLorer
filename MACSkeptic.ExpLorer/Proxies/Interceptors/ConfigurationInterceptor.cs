using System;
using Castle.Core.Interceptor;
using MACSkeptic.ExpLorer.Utils.Extensions;

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
            if (MethodWasIntercepted(invocation))
            {
                return;
            }

            invocation.Proceed();
            return;
        }

        private bool MethodWasIntercepted(IInvocation invocation)
        {
            var method = invocation.Method;
            var name = method.WhatGives();

            if (name.IsEmpty())
            {
                return false;
            }

            var type = method.ReturnType;

            return ItReturnsAnInterface(type, name, invocation) ||
                   ItReturnsAString(type, name, invocation);
        }

        private bool ItReturnsAnInterface(Type type, string name, IInvocation invocation)
        {
            if (!type.IsInterface)
            {
                return false;
            }

            invocation.ReturnValue = ConfigurationProxy.For(type, _configuration.Get(name));
            return true;
        }

        private bool ItReturnsAString(Type type, string name, IInvocation invocation)
        {
            if (typeof (string) != type)
            {
                return false;
            }

            invocation.ReturnValue = _configuration.Get(name).Value;
            return true;
        }
    }
}