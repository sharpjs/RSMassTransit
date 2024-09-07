// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

#if PORTED
using Autofac;
using FluentAssertions;
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
                    .Should().BeSameAs(ReportingServicesClientFactory.Instance);
            }
        }
    }
}
#endif
