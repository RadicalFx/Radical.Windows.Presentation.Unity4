using Microsoft.Practices.Unity;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Services
{
    class ComponentReleaser : IReleaseComponents
    {
        readonly IUnityContainer container;

        public ComponentReleaser(IUnityContainer container)
        {
            this.container = container;
        }

        public void Release(object component)
        {
            if (component != null)
            {
                this.container.Teardown(component);
            }
        }
    }
}
