using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using Sharp.ServiceHost;

namespace RSMassTransit
{
    [RunInstaller(true)]
    [DesignerCategory("")] // disable designer
    internal class RSMassTransitServiceInstaller : Installer
    {
        public RSMassTransitServiceInstaller()
        {
            Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });

            Installers.Add(new ServiceInstaller
            {
                ServiceName = RSMassTransitService.Name,
                DisplayName = RSMassTransitService.DisplayName,
                Description = RSMassTransitService.Description,
                StartType   = ServiceStartMode.Automatic,
            }
            .WithInstanceId(Configuration.Current.InstanceId));
        }
    }
}
