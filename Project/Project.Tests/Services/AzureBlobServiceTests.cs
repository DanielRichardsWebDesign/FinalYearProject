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
            var filePath = @"Images/test-image.png";

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
            //Test container
            var testContainerName = "test";

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Blob continuation token
            BlobContinuationToken blobContinuationToken = null;

            //Get Blob container
            var blobContainer = blobClient.GetContainerReference(testContainerName);
            var response = blobContainer.ListBlobsSegmentedAsync(blobContinuationToken);

            //Iterate through blobs on container
            foreach(IListBlobItem blob in blobContainer.ListBlobs())
            {
                if(blob.GetType() == typeof(CloudBlockBlob))
                {
                    ((CloudBlockBlob)blob).DeleteIfExistsAsync();
                }                    
            }

            Assert.AreEqual(blobContainer.ListBlobs().Count(), 0);
        }

        [TestMethod()]
        public void DownloadAsyncTest()
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

            //Download blob to a local directory
            Stream file = File.OpenWrite(@"C:\Users\Daniel Richards\Documents\University" + blob);
            blobOnContainer.DownloadToStream(file);

            //Assert not empty
            Assert.IsNotNull(file);
        }

        [TestMethod()]
        public void DownloadRepositoryAsyncTest()
        {
            //Test container
            var testContainerName = "test";

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get Blob container
            var blobContainer = blobClient.GetContainerReference(testContainerName);

            //List blobs in container
            var listOfBlobs = blobContainer.ListBlobs();

            //Iterate and add to new list
            List<CloudBlockBlob> blobsList = new List<CloudBlockBlob>();

            foreach(var item in listOfBlobs)
            {
                CloudBlockBlob blob = (CloudBlockBlob)item;
                blob.FetchAttributes();

                blobsList.Add(blob);
            }

            //Assert list isn't empty - Meaning that these items can be downloaded
            Assert.IsNotNull(blobsList);
        }

        [TestMethod()]
        public void DownloadSpecifiedFilesTest()
        {
            //Test container
            var testContainerName = "test";

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get Blob container
            var blobContainer = blobClient.GetContainerReference(testContainerName);

            //File list
            List<string> selectedFiles = new List<string>();
            string[] files = { "test1.jpg", "test2.mp4", "test3.mp3" };

            foreach(var file in files)
            {
                selectedFiles.Add(file);
            }

            //List blobs in container
            //var listOfBlobs = blobContainer.ListBlobs();
            //List<CloudBlockBlob> blobsList = new List<CloudBlockBlob>();

            List<CloudBlockBlob> blobsList = new List<CloudBlockBlob>();

            //Iterate through and select files from container
            foreach(var item in selectedFiles)
            {
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(item);
                blob.FetchAttributes();

                blobsList.Add(blob);
            }

            //Assert that the blobs list is not null - SO that they can then be downloaded
            Assert.IsNotNull(blobsList);
        }
    }
}