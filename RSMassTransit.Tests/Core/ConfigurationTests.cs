#if PORTED
/*
    Copyright 2020 Jeffrey Sharp

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

using System;
using System.Collections.Specialized;
using System.Configuration;
using FluentAssertions;
using NUnit.Framework;
using RSMassTransit.Storage;

namespace RSMassTransit.Core
{
    [TestFixture]
    public class ConfigurationTests
    {
        private static readonly NameValueCollection
            Defaults = new NameValueCollection();

        // Constructor

        [Test]
        public void Construct_NullSettings()
        {
            this.Invoking(_ => new Configuration(null))
                .Should().Throw<ArgumentNullException>();
        }

        // InstanceId

        [Test]
        public void InstanceId_Current()
        {
            Configuration.Current.InstanceId
                .Should().Be("TestInstance");
        }

        [Test]
        public void InstanceId_Default()
        {
            new Configuration(Defaults).InstanceId
                .Should().BeEmpty();
        }

        [Test]
        public void InstanceId_Override()
        {
            var settings = new NameValueCollection { ["InstanceId"] = "A" };

            new Configuration(settings).InstanceId
                .Should().Be("A");
        }

        // BusUri

        [Test]
        public void BusUri_Current()
        {
           Configuration.Current.BusUri
                .Should().Be(new Uri("test://bus"));
        }

        [Test]
        public void BusUri_Default()
        {
            new Configuration(Defaults).BusUri
                .Should().Be(new Uri("rabbitmq://localhost"));
        }

        [Test]
        public void BusUri_Override()
        {
            var settings = new NameValueCollection { ["BusUri"] = "urn:a" };

            new Configuration(settings).BusUri
                .Should().Be(new Uri("urn:a"));
        }

        [Test]
        public void BusUri_Invalid()
        {
            var settings = new NameValueCollection { ["BusUri"] = "not a URI" };

            this.Invoking(_ => new Configuration(settings))
                .Should().Throw<ConfigurationErrorsException>()
                .Which.Message.Should().Contain("The value must be an absolute URI.");
        }

        // BusQueue

        [Test]
        public void BusQueue_Current()
        {
            Configuration.Current.BusQueue
                .Should().Be("testqueue");
        }

        [Test]
        public void BusQueue_Default()
        {
            new Configuration(Defaults).BusQueue
                .Should().Be("reports");
        }

        [Test]
        public void BusQueue_Override()
        {
            var settings = new NameValueCollection { ["BusQueue"] = "a" };

            new Configuration(settings).BusQueue
                .Should().Be("a");
        }

        // BusSecretName

        [Test]
        public void BusSecretName_Current()
        {
            Configuration.Current.BusSecretName
                .Should().Be("testing");
        }

        [Test]
        public void BusSecretName_Default()
        {
            new Configuration(Defaults).BusSecretName
                .Should().Be("guest");
        }

        [Test]
        public void BusSecretName_Override()
        {
            var settings = new NameValueCollection { ["BusSecretName"] = "a" };

            new Configuration(settings).BusSecretName
                .Should().Be("a");
        }

        // BusSecret

        [Test]
        public void BusSecret_Current()
        {
            Configuration.Current.BusSecret
                .Should().Be("12345");
        }

        [Test]
        public void BusSecret_Default()
        {
            new Configuration(Defaults).BusSecret
                .Should().Be("guest");
        }

        [Test]
        public void BusSecret_Override()
        {
            var settings = new NameValueCollection { ["BusSecret"] = "a" };

            new Configuration(settings).BusSecret
                .Should().Be("a");
        }

        // StorageType

        [Test]
        public void StorageType_Current()
        {
            Configuration.Current.StorageType
                .Should().Be(StorageType.AzureBlob);
        }

        [Test]
        public void StorageType_Default()
        {
            new Configuration(Defaults).StorageType
                .Should().Be(StorageType.File);
        }

        [Test]
        public void StorageType_Override()
        {
            var settings = new NameValueCollection { ["Storage.Type"] = "AzureBlob" };

            new Configuration(settings).StorageType
                .Should().Be(StorageType.AzureBlob);
        }

        [Test]
        public void StorageType_Invalid()
        {
            var settings = new NameValueCollection { ["Storage.Type"] = "not a storage type" };

            this.Invoking(_ => new Configuration(settings))
                .Should().Throw<ConfigurationErrorsException>()
                .Which.Message.Should().Contain("The value must be one of:");
        }

        // Storage.File.Path

        [Test]
        public void Storage_File_Path_Current()
        {
            Configuration.Current.File.Path
                .Should().Be(@"T:\TestBlobs");
        }

        [Test]
        public void Storage_File_Path_Default()
        {
            new Configuration(Defaults).File.Path
                .Should().Be(@"C:\Blobs");
        }

        [Test]
        public void Storage_File_Path_Override()
        {
            var settings = new NameValueCollection { ["Storage.File.Path"] = @"T:\" };

            new Configuration(settings).File.Path
                .Should().Be(@"T:\");
        }

        // Storage.File.ReadBufferSize

        [Test]
        public void Storage_File_ReadBufferSize_Current()
        {
            Configuration.Current.File.ReadBufferSize
                .Should().Be(68000);
        }

        [Test]
        public void Storage_File_ReadBufferSize_Default()
        {
            new Configuration(Defaults).File.ReadBufferSize
                .Should().BeNull();
        }

        [Test]
        public void Storage_File_ReadBufferSize_Override()
        {
            var settings = new NameValueCollection { ["Storage.File.ReadBufferSize"] = @"6502" };

            new Configuration(settings).File.ReadBufferSize
                .Should().Be(6502);
        }

        [Test]
        public void Storage_File_ReadBufferSize_Invalid()
        {
            var settings = new NameValueCollection { ["Storage.File.ReadBufferSize"] = @"not a number" };

            this.Invoking(_ => new Configuration(settings))
                .Should().Throw<ConfigurationErrorsException>()
                .Which.Message.Should().Contain("The value must be an integer,");
        }

        // Storage.File.WriteBufferSize

        [Test]
        public void Storage_File_WriteBufferSize_Current()
        {
            Configuration.Current.File.WriteBufferSize
                .Should().Be(80386);
        }

        [Test]
        public void Storage_File_WriteBufferSize_Default()
        {
            new Configuration(Defaults).File.WriteBufferSize
                .Should().BeNull();
        }

        [Test]
        public void Storage_File_WriteBufferSize_Override()
        {
            var settings = new NameValueCollection { ["Storage.File.WriteBufferSize"] = @"6502" };

            new Configuration(settings).File.WriteBufferSize
                .Should().Be(6502);
        }

        [Test]
        public void Storage_File_WriteBufferSize_Invalid()
        {
            var settings = new NameValueCollection { ["Storage.File.WriteBufferSize"] = @"not a number" };

            this.Invoking(_ => new Configuration(settings))
                .Should().Throw<ConfigurationErrorsException>()
                .Which.Message.Should().Contain("The value must be an integer,");
        }

        // Storage.AzureBlob.ConnectionString

        [Test]
        public void Storage_AzureBlob_ConnectionString_Current()
        {
            Configuration.Current.Azure.ConnectionString
                .Should().Be("Testing=true");
        }

        [Test]
        public void Storage_AzureBlob_ConnectionString_Default()
        {
            new Configuration(Defaults).Azure.ConnectionString
                .Should().Be("UseDevelopmentStorage=true");
        }

        [Test]
        public void Storage_AzureBlob_ConnectionString_Override()
        {
            var settings = new NameValueCollection { ["Storage.AzureBlob.ConnectionString"] = "A" };

            new Configuration(settings).Azure.ConnectionString
                .Should().Be("A");
        }

        // Storage.AzureBlob.ContainerName

        [Test]
        public void Storage_AzureBlob_ContainerName_Current()
        {
            Configuration.Current.Azure.ContainerName
                .Should().Be("testblobs");
        }

        [Test]
        public void Storage_AzureBlob_ContainerName_Default()
        {
            new Configuration(Defaults).Azure.ContainerName
                .Should().Be("reports");
        }

        [Test]
        public void Storage_AzureBlob_ContainerName_Override()
        {
            var settings = new NameValueCollection { ["Storage.AzureBlob.ContainerName"] = "a" };

            new Configuration(settings).Azure.ContainerName
                .Should().Be("a");
        }
    }
}
#endif
