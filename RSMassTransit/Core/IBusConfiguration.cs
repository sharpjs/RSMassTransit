// Copyright (C) 2018 (to be determined)

using System;

namespace RSMassTransit
{
    internal interface IBusConfiguration
    {
        Uri    BusUri        { get; }
        string BusQueue      { get; }
        string BusSecret     { get; }
        string BusSecretName { get; }
    }
}
