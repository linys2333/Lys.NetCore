using Lys.NetCore.Infrastructure.Extensions;

namespace Lys.NetCore.Infrastructure.Services
{
    /// <summary>
    /// 延迟实例化类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyService<T> where T : class
    {
        private T m_Instance;

        public T NewInstance => IoCExtensions.GetService<T>();

        public T Instance => m_Instance ?? (m_Instance = NewInstance);
    }
}
