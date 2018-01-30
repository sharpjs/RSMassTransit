using Autofac;

namespace RSMassTransit.ReportingServices
{
    internal class ReportingServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(new ReportingServicesClientFactory())
                .As<IReportingServicesClientFactory>();
        }
    }
}
