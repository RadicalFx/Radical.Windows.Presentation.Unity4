using Microsoft.Practices.Unity;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class UnityApplicationBootstrapper<TShellView> :
        UnityApplicationBootstrapper
        where TShellView : Window
    {
        public UnityApplicationBootstrapper()
        {
            this.UsingAsShell<TShellView>();
        }

        public UnityApplicationBootstrapper(IUnityContainer existingContainer)
            : base(existingContainer)
        {
            this.UsingAsShell<TShellView>();
        }
    }
}
