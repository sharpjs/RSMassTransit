﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace RSMassTransit.Storage
{
    internal interface IBlobRepository
    {
        Task<Uri> PutAsync(Stream stream);
    }
}