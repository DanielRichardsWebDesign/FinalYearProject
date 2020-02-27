using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Project.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            //Test container
            var testContainerName = "test";

            //Sample blob name
            var blob = "test-image.png";

            //Path to file
            var filePath = "C:/Users/Daniel Richards/Pictures/test-image.png";

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get Blob container
            var blobContainer = blobClient.GetContainerReference(testContainerName);

            //Open File and upload its data
            FileStream uploadFileStream = File.OpenRead(filePath);
            var blobOnContainer = blobContainer.GetBlockBlobReference(blob);
            blobOnContainer.UploadFromFile(filePath);

            //Assert blob exists on container
            Assert.IsNotNull(blobOnContainer);
        }

        [TestMethod()]
        public void DeleteAsyncTest()
        {
            //Test container
            var testContainerName = "test";

            //Sample blob name
            var blob = "test-image.png";

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get Blob container
            var blobContainer = blobClient.GetContainerReference(testContainerName);

            //Get blob on container
            var blobOnContainer = blobContainer.GetBlockBlobReference(blob);

            //Delete blob on container
            blobOnContainer.DeleteIfExists();

            Assert.IsTrue(!blobOnContainer.Exists());
        }

        [TestMethod()]
        public void DeleteAllAsyncTest()
        {
            Assert.Fail();
        }
    }
}