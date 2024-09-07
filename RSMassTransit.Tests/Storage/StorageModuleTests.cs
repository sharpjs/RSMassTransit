// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

#if PORTED
using Autofac;
using Autofac.Core;
using FluentAssertions;
using NUnit.Framework;
using Sharp.BlobStorage;

namespace RSMassTransit.Storage
{
    [TestFixture]
    public class StorageModuleTests
    {
        [Test]
        public void Provides_IBlobStorage()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new StorageModule());

            using (var container = builder.Build())
            {
                container.ComponentRegistry
                    .IsRegistered(new TypedService(typeof(IBlobStorage)))
                    .Should().BeTrue();
            }
        }
    }
}
#endif
