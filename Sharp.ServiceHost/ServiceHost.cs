/*
    Copyright (C) 2019 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

using System;
using System.Configuration.Install;
using System.Reflection;

namespace Sharp.ServiceHost
{
    public static class ServiceHost<TService>
        where TService : Service, new()
    {
        public static void Run(string[] args)
        {
            var command = (args != null && args.Length > 0)
                ? args[0]
                : null;

            if (command == "/install")
                // Install Windows service
                Install();
            else if (command == "/uninstall")
                // Uninstall Windows service
                Uninstall();
            else if (command == "/console")
                // Run as a normal console app, NOT as a Windows service.
                RunNonInteractive();
            else if (command != null)
                // Unrecognized command
                Environment.Exit(1);
            else if (Environment.UserInteractive)
                // Run in a console window, waiting for a keypress to stop.
                RunInteractive();
            else
                // Run as a Windows service
                RunService();

            Environment.Exit(0);
        }

        private static void RunService()
        {
            new TService().RunService();
        }

        private static void RunInteractive()
        {
            new TService().RunInteractive();
        }

        private static void RunNonInteractive()
        {
            new TService().RunNonInteractive();
        }

        private static void Install()
        {
            ManagedInstallerClass.InstallHelper(new[]
            {
                Assembly.GetEntryAssembly().Location
            });
        }

        private static void Uninstall()
        {
            ManagedInstallerClass.InstallHelper(new[]
            {
                "/u", // uninstall
                Assembly.GetEntryAssembly().Location
            });
        }
    }
}
