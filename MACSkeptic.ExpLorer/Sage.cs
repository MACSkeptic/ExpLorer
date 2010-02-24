﻿using System;
using System.IO;
using System.Reflection;
using MACSkeptic.ExpLorer.Parsers;

namespace MACSkeptic.ExpLorer
{
    public class Sage
    {
        private readonly IConfigurationParser _parser;
        private readonly IConfigurationProxyProvider _proxy;

        public Sage(IConfigurationParser parser, IConfigurationProxyProvider proxy)
        {
            _parser = parser;
            _proxy = proxy;
        }

        public T CreateProxy<T>()
            where T : class
        {
            return (T)CreateProxyFor(typeof (T));
        }

        public object CreateProxyFor(Type type)
        {
            return _proxy.For(
                type,
                _parser.LoadFromFile(
                    Path.Combine(
                        new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", string.Empty)).Directory.FullName,
                        type.Name.Substring(1))));
        }
    }
}