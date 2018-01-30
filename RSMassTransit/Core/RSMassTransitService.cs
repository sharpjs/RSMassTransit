using Sharp.ServiceHost;

namespace RSMassTransit.Core
{
    internal class RSMassTransitService : Service
    {
        public const string
            Name        = "RSMassTransit",
            DisplayName = "SQL Server Reporting Services MassTransit Interface",
            Description = "Executes reports in response to messages received on a MassTransit message bus.";

        public RSMassTransitService()
            : base(Name) { }

        protected override void StartCore()
        {
            base.StartCore();
        }

        protected override void StopCore()
        {
            base.StopCore();
        }
    }
}
