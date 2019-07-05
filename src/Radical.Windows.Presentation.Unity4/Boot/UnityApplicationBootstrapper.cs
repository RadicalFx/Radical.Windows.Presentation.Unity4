using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Topics.Radical.Windows.Presentation.Extensions;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class UnityApplicationBootstrapper : ApplicationBootstrapper
    {
        IUnityContainer container;
        bool ownContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityApplicationBootstrapper"/> class.
        /// </summary>
        public UnityApplicationBootstrapper()
        {
            ownContainer = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityApplicationBootstrapper"/> class.
        /// </summary>
        /// <param name="existingContainer">The externally managed container to use.</param>
        public UnityApplicationBootstrapper(IUnityContainer existingContainer)
        {
            container = existingContainer;
            ownContainer = false;
        }

        protected override IServiceProvider CreateServiceProvider()
        {
            if (ownContainer)
            {
                container = new UnityContainer();
            }

            var wrapper = new ServiceProviderWrapper(container, ownContainer);
            container.RegisterInstance<IServiceProvider>(wrapper);

            if (!container.IsRegistered<IUnityContainer>())
            {
                container.RegisterInstance<IUnityContainer>(container);
            }

            if (!container.IsRegistered<BootstrapConventions>())
            {
                container.RegisterType<BootstrapConventions>(new ContainerControlledLifetimeManager());
            }

            container.AddNewExtension<SubscribeToMessageExtension>();
            container.AddNewExtension<InjectViewInRegionExtension>();

            return wrapper;
        }

        [ImportMany]
        IEnumerable<IUnityInstaller> Installers { get; set; }

        protected override void OnCompositionContainerComposed(CompositionContainer container, IServiceProvider serviceProvider)
        {
            base.OnCompositionContainerComposed(container, serviceProvider);

            var toInstall = this.Installers.Where(i => this.ShouldInstall(i)).ToArray();

            var conventions = this.container.Resolve<Boot.BootstrapConventions>();

            var allTypes = new HashSet<Type>(Assembly.GetEntryAssembly().GetTypes());
            foreach (var dll in Directory.EnumerateFiles(Helpers.EnvironmentHelper.GetCurrentDirectory(), "*.dll"))
            {
                var name = Path.GetFileNameWithoutExtension(dll);
                var a = Assembly.Load(name);
                if (conventions.IncludeAssemblyInContainerScan(a))
                {
                    var ts = a.GetTypes();
                    foreach (var t in ts)
                    {
                        allTypes.Add(t);
                    }
                }
            }

            foreach (var installer in toInstall)
            {
                installer.Install(this.container, conventions, allTypes);
            }
        }

        protected virtual Boolean ShouldInstall(IUnityInstaller installer)
        {
            return true;
        }

        protected override IEnumerable<T> ResolveAll<T>()
        {
            return this.container.ResolveAll<T>();
        }
    }
}
