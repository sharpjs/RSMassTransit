// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

#if PORTED
using System;
using System.Collections.Specialized;
using System.Configuration;
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
            Should.Throw<ArgumentNullException>(() => new Configuration(null));
        }

        // InstanceId

        [Test]
        public void InstanceId_Current()
        {
            Configuration.Current.InstanceId
                .ShouldBe("TestInstance");
        }

        [Test]
        public void InstanceId_Default()
        {
            new Configuration(Defaults).InstanceId
                .ShouldBeEmpty();
        }

        [Test]
        public void InstanceId_Override()
        {
            var settings = new NameValueCollection { ["InstanceId"] = "A" };

            new Configuration(settings).InstanceId
                .ShouldBe("A");
        }

        // BusUri

        [Test]
        public void BusUri_Current()
        {
           Configuration.Current.BusUri
                .ShouldBe(new Uri("test://bus"));
        }

        [Test]
        public void BusUri_Default()
        {
            new Configuration(Defaults).BusUri
                .ShouldBe(new Uri("rabbitmq://localhost"));
        }

        [Test]
        public void BusUri_Override()
        {
            var settings = new NameValueCollection { ["BusUri"] = "urn:a" };

            new Configuration(settings).BusUri
                .ShouldBe(new Uri("urn:a"));
        }

        [Test]
        public void BusUri_Invalid()
        {
            var settings = new NameValueCollection { ["BusUri"] = "not a URI" };

            Should.Throw<ConfigurationErrorsException>(() => new Configuration(settings))
                .Message.ShouldContain("The value must be an absolute URI.");
        }

        // BusQueue

        [Test]
        public void BusQueue_Current()
        {
            Configuration.Current.BusQueue
                .ShouldBe("testqueue");
        }

        [Test]
        public void BusQueue_Default()
        {
            new Configuration(Defaults).BusQueue
                .ShouldBe("reports");
        }

        [Test]
        public void BusQueue_Override()
        {
            var settings = new NameValueCollection { ["BusQueue"] = "a" };

            new Configuration(settings).BusQueue
                .ShouldBe("a");
        }

        // BusSecretName

        [Test]
        public void BusSecretName_Current()
        {
            Configuration.Current.BusSecretName
                .ShouldBe("testing");
        }

        [Test]
        public void BusSecretName_Default()
        {
            new Configuration(Defaults).BusSecretName
                .ShouldBe("guest");
        }

        [Test]
        public void BusSecretName_Override()
        {
            var settings = new NameValueCollection { ["BusSecretName"] = "a" };

            new Configuration(settings).BusSecretName
                .ShouldBe("a");
        }

        // BusSecret

        [Test]
        public void BusSecret_Current()
        {
            Configuration.Current.BusSecret
                .ShouldBe("12345");
        }

        [Test]
        public void BusSecret_Default()
        {
            new Configuration(Defaults).BusSecret
                .ShouldBe("guest");
        }

        [Test]
        public void BusSecret_Override()
        {
            var settings = new NameValueCollection { ["BusSecret"] = "a" };

            new Configuration(settings).BusSecret
                .ShouldBe("a");
        }

        // StorageType

        [Test]
        public void StorageType_Current()
        {
            Configuration.Current.StorageType
                .ShouldBe(StorageType.AzureBlob);
        }

        [Test]
        public void StorageType_Default()
        {
            new Configuration(Defaults).StorageType
                .ShouldBe(StorageType.File);
        }

        [Test]
        public void StorageType_Override()
        {
            var settings = new NameValueCollection { ["Storage.Type"] = "AzureBlob" };

            new Configuration(settings).StorageType
                .ShouldBe(StorageType.AzureBlob);
        }

        [Test]
        public void StorageType_Invalid()
        {
            var settings = new NameValueCollection { ["Storage.Type"] = "not a storage type" };

            Should.Throw<ConfigurationErrorsException>(() => new Configuration(settings))
                .Message.ShouldContain("The value must be one of:");
        }

        // Storage.File.Path

        [Test]
        public void Storage_File_Path_Current()
        {
            Configuration.Current.File.Path
                .ShouldBe(@"T:\TestBlobs");
        }

        [Test]
        public void Storage_File_Path_Default()
        {
            new Configuration(Defaults).File.Path
                .ShouldBe(@"C:\Blobs");
        }

        [Test]
        public void Storage_File_Path_Override()
        {
            var settings = new NameValueCollection { ["Storage.File.Path"] = @"T:\" };

            new Configuration(settings).File.Path
                .ShouldBe(@"T:\");
        }

        // Storage.File.ReadBufferSize

        [Test]
        public void Storage_File_ReadBufferSize_Current()
        {
            Configuration.Current.File.ReadBufferSize
                .ShouldBe(68000);
        }

        [Test]
        public void Storage_File_ReadBufferSize_Default()
        {
            new Configuration(Defaults).File.ReadBufferSize
                .ShouldBeNull();
        }

        [Test]
        public void Storage_File_ReadBufferSize_Override()
        {
            var settings = new NameValueCollection { ["Storage.File.ReadBufferSize"] = @"6502" };

            new Configuration(settings).File.ReadBufferSize
                .ShouldBe(6502);
        }

        [Test]
        public void Storage_File_ReadBufferSize_Invalid()
        {
            var settings = new NameValueCollection { ["Storage.File.ReadBufferSize"] = @"not a number" };

            Should.Throw<ConfigurationErrorsException>(() => new Configuration(settings))
                .Message.ShouldContain("The value must be an integer,");
        }

        // Storage.File.WriteBufferSize

        [Test]
        public void Storage_File_WriteBufferSize_Current()
        {
            Configuration.Current.File.WriteBufferSize
                .ShouldBe(80386);
        }

        [Test]
        public void Storage_File_WriteBufferSize_Default()
        {
            new Configuration(Defaults).File.WriteBufferSize
                .ShouldBeNull();
        }

        [Test]
        public void Storage_File_WriteBufferSize_Override()
        {
            var settings = new NameValueCollection { ["Storage.File.WriteBufferSize"] = @"6502" };

            new Configuration(settings).File.WriteBufferSize
                .ShouldBe(6502);
        }

        [Test]
        public void Storage_File_WriteBufferSize_Invalid()
        {
            var settings = new NameValueCollection { ["Storage.File.WriteBufferSize"] = @"not a number" };

            Should.Throw<ConfigurationErrorsException>(() => new Configuration(settings))
                .Message.ShouldContain("The value must be an integer,");
        }

        // Storage.AzureBlob.ConnectionString

        [Test]
        public void Storage_AzureBlob_ConnectionString_Current()
        {
            Configuration.Current.Azure.ConnectionString
                .ShouldBe("Testing=true");
        }

        [Test]
        public void Storage_AzureBlob_ConnectionString_Default()
        {
            new Configuration(Defaults).Azure.ConnectionString
                .ShouldBe("UseDevelopmentStorage=true");
        }

        [Test]
        public void Storage_AzureBlob_ConnectionString_Override()
        {
            var settings = new NameValueCollection { ["Storage.AzureBlob.ConnectionString"] = "A" };

            new Configuration(settings).Azure.ConnectionString
                .ShouldBe("A");
        }

        // Storage.AzureBlob.ContainerName

        [Test]
        public void Storage_AzureBlob_ContainerName_Current()
        {
            Configuration.Current.Azure.ContainerName
                .ShouldBe("testblobs");
        }

        [Test]
        public void Storage_AzureBlob_ContainerName_Default()
        {
            new Configuration(Defaults).Azure.ContainerName
                .ShouldBe("reports");
        }

        [Test]
        public void Storage_AzureBlob_ContainerName_Override()
        {
            var settings = new NameValueCollection { ["Storage.AzureBlob.ContainerName"] = "a" };

            new Configuration(settings).Azure.ContainerName
                .ShouldBe("a");
        }
    }
}
#endif
