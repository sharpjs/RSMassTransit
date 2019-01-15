/*
    Copyright (C) 2019 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

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
