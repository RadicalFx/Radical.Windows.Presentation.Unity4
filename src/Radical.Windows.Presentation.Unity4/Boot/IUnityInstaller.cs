using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Topics.Radical.Windows.Presentation.Boot
{
    [InheritedExport]
    public interface IUnityInstaller
    {
        void Install(IUnityContainer container, BootstrapConventions conventions, IEnumerable<Type> allTypes);
    }
}
