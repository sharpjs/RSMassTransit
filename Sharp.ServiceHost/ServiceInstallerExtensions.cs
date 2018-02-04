// Copyright (C) 2018 (to be determined)

using System;
using System.ServiceProcess;

namespace Sharp.ServiceHost
{
    public static class ServiceInstallerExtensions
    {
        public static ServiceInstaller WithInstanceId(this ServiceInstaller installer, string instanceId)
        {
            if (installer == null)
                throw new ArgumentNullException("installer");

            if (!string.IsNullOrWhiteSpace(instanceId))
            {
                installer.ServiceName = string.Concat(installer.ServiceName,  ".", instanceId     );
                installer.DisplayName = string.Concat(installer.DisplayName, " (", instanceId, ")");
            }

            return installer;
        }
    }
}
