using Microsoft.Owin;
using Owin;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.StockTicker;
using Microsoft.Owin.Security.Provider;
using StructureMap;
using Owin;

[assembly: OwinStartup(typeof(SignalRStructureMapWalkthrough.Startup), "Configuration")]

namespace SignalRStructureMapWalkthrough
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            var container = ObjectFactory.Container;
            var resolver = new StructureMapSignalRDependencyResolver(container);

            ObjectFactory.Configure(x =>
            {

                x.For<Microsoft.AspNet.SignalR.StockTicker.IStockTicker>()
                    .Singleton()
                    .Use<Microsoft.AspNet.SignalR.StockTicker.StockTicker>();

                x.For<IHubConnectionContext>().ConditionallyUse(c =>
                    c.If(t => t.ParentType.GetInterface(typeof(Microsoft.AspNet.SignalR.StockTicker.IStockTicker).Name) ==
                        typeof(Microsoft.AspNet.SignalR.StockTicker.IStockTicker))
                        .ThenIt.Is.ConstructedBy(
                            () => resolver.Resolve<IConnectionManager>().GetHubContext<StockTickerHub>().Clients)
                    );
            });

            var config = new HubConfiguration()
            {
                Resolver = resolver
            };


            Microsoft.AspNet.SignalR.StockTicker.Startup.ConfigureSignalR(app, config);
        }
    }

    public class StructureMapSignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IContainer _container;
        public StructureMapSignalRDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            object service = null;
            //Below is a key difference between this StructureMap example, GetInstance is used for concrete classes.
            if (!serviceType.IsAbstract && !serviceType.IsInterface && serviceType.IsClass)
            {
                //If the type is a concreate type we get here...
                service = _container.GetInstance(serviceType);
            }
            else
            {
                // Non concrete resolution which uses the base dependency resolver if needed.
                service = _container.TryGetInstance(serviceType) ?? base.GetService(serviceType);
            }
            return service;
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var objects = _container.GetAllInstances(serviceType).Cast<object>();
            return objects.Concat(base.GetServices(serviceType));
        }
    }
}