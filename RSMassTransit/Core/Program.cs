// Copyright (C) 2018 (to be determined)

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
