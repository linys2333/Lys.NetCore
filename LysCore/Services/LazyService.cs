using LysCore.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LysCore.Services
{
    public class LysService
    {
        public static IServiceProvider ServiceProvider;
    }

    public class LazyService<T> : LysService where T : class
    {
        private T m_Instance;

        public T NewInstance
        {
            get
            {
                Requires.NotNull(ServiceProvider, nameof(ServiceProvider));
                return ActivatorUtilities.GetServiceOrCreateInstance<T>(ServiceProvider);
            }
        }

        public T Instance => m_Instance ?? (m_Instance = NewInstance);
    }
}
