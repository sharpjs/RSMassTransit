using Sharp.ServiceHost;

namespace RSMassTransit.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceHost<RSMassTransitService>.Run(args);
        }
    }
}
