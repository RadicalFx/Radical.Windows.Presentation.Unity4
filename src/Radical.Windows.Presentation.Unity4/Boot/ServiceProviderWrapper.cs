﻿using Microsoft.Practices.Unity;
using System;

namespace Topics.Radical.Windows.Presentation.Boot
{
    class ServiceProviderWrapper : IServiceProvider, IDisposable
    {
        bool ownContainer;

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ServiceProviderWrapper"/> is reclaimed by garbage collection.
        /// </summary>
        ~ServiceProviderWrapper()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (ownContainer)
            {
                this.Container?.Dispose();
            }

            this.Container = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderWrapper" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="ownContainer">True if the container is entirely managed by Radical; Otherwise, false..</param>
        public ServiceProviderWrapper(IUnityContainer container, bool ownContainer)
        {
            Container = container;
            this.ownContainer = ownContainer;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType" />.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (this.Container.IsRegistered(serviceType))
            {
                return this.Container.Resolve(serviceType);
            }

            return null;
        }
    }
}
