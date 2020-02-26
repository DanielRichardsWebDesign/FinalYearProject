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
        public void CreateBlobContainerTest()
        {
            var testContainerName = "test";

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Check if a blob container of that name exists
            var blobContainer = blobClient.GetContainerReference(testContainerName);

            //Create new Container if it does not exist
            blobContainer.CreateIfNotExists();

            Assert.IsTrue(blobContainer.Exists());
        }

        [TestMethod()]
        public void GetBlobContainerTest()
        {
            //Test container
            var testContainerName = "test";

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get Blob container
            var blobContainer = blobClient.GetContainerReference(testContainerName);

            //Assert that container exists
            Assert.IsTrue(blobContainer.Exists());
        }

        [TestMethod()]
        public void GetClient()
        {
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Asert that the blob client exists
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