using System;
using Microsoft.Xna.Framework;

namespace RC.Engine.Base
{
    public class RCServiceProvider : IServiceProvider
    {
        private IServiceProvider _serviceProvider;

        public RCServiceProvider(IServiceProvider services)
        {
            _serviceProvider = services;
        }

        public T GetService<T>()
        {
            Type type = typeof(T);
            return (T)GetService(type);
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        #endregion
    }
}
