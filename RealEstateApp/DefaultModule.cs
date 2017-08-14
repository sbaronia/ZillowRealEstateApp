using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateApp.Services;
using Microsoft.Extensions.Logging;

namespace RealEstateApp
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register( c => new ZillowRealEstateServiceImpl(c.Resolve<ILogger<ZillowRealEstateServiceImpl>>())).As<IRealEstateService>().InstancePerLifetimeScope();
        }
    }
}
