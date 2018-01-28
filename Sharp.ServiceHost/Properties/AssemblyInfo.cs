using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information
[assembly: AssemblyTitle       ("Sharp.ServiceHost")]
[assembly: AssemblyDescription ("Micro-framework that enables a console app to run as a Windows service")]
[assembly: AssemblyProduct     ("Sharp.ServiceHost")]
[assembly: AssemblyCompany     ("(to be determined)")]
[assembly: AssemblyCopyright   ("Copyright 2018 (to be determined)")]
[assembly: AssemblyVersion     ("0.0.0")]

// Compliance
[assembly: ComVisible(false)]

// Security
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: InternalsVisibleTo("Sharp.ServiceHost.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
                            // ^^^^^^^^^^^^^^^^^^^^^^^^
                            // Required for Moq to mock a class with an internal abstract method.

// Configuration
#if DEBUG
    [assembly: AssemblyConfiguration("Debug")]
#else
    [assembly: AssemblyConfiguration("Release")]
#endif
