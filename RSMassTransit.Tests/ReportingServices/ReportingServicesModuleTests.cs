// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

#if PORTED
using Autofac;
using NUnit.Framework;

namespace RSMassTransit.ReportingServices
{
    [TestFixture]
    public class ReportingServicesModuleTests
    {
        [Test]
        public void Provides_IReportingServicesClientFactory()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ReportingServicesModule());

            using (var container = builder.Build())
            {
                container
                    .Resolve<IReportingServicesClientFactory>()
                    .ShouldBeSameAs(ReportingServicesClientFactory.Instance);
            }
        }
    }
}
#endif
