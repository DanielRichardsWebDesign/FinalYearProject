using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Project.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Tests
{
    [TestClass()]
    public class AzureBlobServiceTests
    {
        [TestMethod()]
        public void GetBlobContainerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetClient()
        {
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            Assert.IsNotNull(blobClient);
        }

        [TestMethod()]
        public void UploadAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAllAsyncTest()
        {
            Assert.Fail();
        }
    }
}