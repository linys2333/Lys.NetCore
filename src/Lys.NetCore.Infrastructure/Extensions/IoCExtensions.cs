using System.Collections.Generic;
using Autofac;

namespace Lys.NetCore.Infrastructure.Extensions
{
    public static class IoCExtensions
    {
        public static IContainer Container;
        
        public static T GetService<T>() where T : class
        {
            return Container.ResolveOptional<T>();
        }

        public static IEnumerable<T> GetServices<T>() where T : class
        {
            return Container.ResolveOptional<IEnumerable<T>>();
        }

        public static T GetServiceNamed<T>(string serviceName) where T : class
        {
            return string.IsNullOrEmpty(serviceName)
                ? default(T)
                : Container.ResolveOptionalNamed<T>(serviceName);
        }

        public static T GetServiceOfInterfaceWithPrefix<T>(string serviceNamePrefix) where T : class
        {
            return serviceNamePrefix == null || !typeof(T).IsInterface
                ? default(T)
                : GetServiceNamed<T>(serviceNamePrefix + typeof(T).Name.TrimStart('I'));
        }
    }
}
