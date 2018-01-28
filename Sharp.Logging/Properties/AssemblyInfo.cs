using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information
[assembly: AssemblyTitle       ("Sharp.Logging")]
[assembly: AssemblyDescription ("Logging micro-framework for the .NET TraceSource API")]
[assembly: AssemblyProduct     ("Sharp.Logging")]
[assembly: AssemblyCompany     ("(to be determined)")]
[assembly: AssemblyCopyright   ("Copyright 2018 (to be determined)")]
[assembly: AssemblyVersion     ("0.0.0")]

// Compliance
[assembly: ComVisible(false)]

// Security
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: InternalsVisibleTo("Sharp.Logging.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
                            // ^^^^^^^^^^^^^^^^^^^^^^^^
                            // Required for Moq to mock a class with an internal abstract method.

// Configuration
#if DEBUG
    [assembly: AssemblyConfiguration("Debug")]
#else
    [assembly: AssemblyConfiguration("Release")]
#endif
