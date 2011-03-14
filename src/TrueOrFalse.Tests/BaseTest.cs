﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NUnit.Framework;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class BaseTest
    {
        private static IContainer _container;

        public BaseTest()
        {
            InitializeContainer();
        }

        private static void InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacCoreModule>();
            _container = builder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }


    }
}
