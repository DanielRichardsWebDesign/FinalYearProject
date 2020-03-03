using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Project.Services
{
    public interface IAzureBlobService
    {
        Task<CloudBlobContainer> CreateBlobContainer(string containerName);
        Task UploadAsync(List<HttpPostedFileBase> files, string containerName);
    }

    public class AzureBlobService : IAzureBlobService
    {
        private string storageConnectionString = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;

        public async Task<CloudBlobContainer> CreateBlobContainer(string containerName)
        {        
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Check if a blob container of that name exists
            var blobContainer = blobClient.GetContainerReference(containerName);

            //Create new container if it does not exist
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }

        public async Task GetBlobContainer()
        {

        }

        public async Task UploadAsync(List<HttpPostedFileBase> files, string containerName)
        {
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get container
            var blobContainer = blobClient.GetContainerReference(containerName);

            //Iterate and add files to container
            Parallel.ForEach(files, file =>
            {
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);
                blob.UploadFromStream(file.InputStream);
                file.InputStream.Close();
            });
        }

        public async Task DeleteAsync(string fileUri)
        {

        }

        public async Task DeleteAllAsync()
        {

        }
    }
}
