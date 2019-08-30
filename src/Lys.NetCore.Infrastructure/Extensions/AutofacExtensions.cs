using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using System;
using System.Linq;
using System.Reflection;

namespace Lys.NetCore.Infrastructure.Extensions
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterAssemblyServices<T>(this ContainerBuilder builder, Assembly[] assemblies,
                Func<Type, string> serviceNameMapping = null) where T : class
        {
            if (!assemblies?.Any() ?? true)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var registerBuilder = builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                .AsSelf();

            if (typeof(T).IsInterface)
            {
                registerBuilder = registerBuilder.AsImplementedInterfaces();
            }

            if (serviceNameMapping != null)
            {
                registerBuilder = registerBuilder.Named<T>(serviceNameMapping);
            }

            return registerBuilder;
        }
    }
}
