using Azure;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Project.Services
{
    public interface IAzureBlobService
    {
        Task<CloudBlobContainer> CreateBlobContainer(string containerName);
        Task UploadAsync(IEnumerable<HttpPostedFileBase> files, string containerName);
        Task<string> GetBlobUri(string fileName, string containerName);
        Task DeleteAsync(string containerName, string fileUri);
        Task<Stream> DownloadAsync(string fileName, string containerName);
        Task<List<CloudBlockBlob>> DownloadRepositoryAsync(string containerName);
        Task<List<CloudBlockBlob>> DownloadSpecifiedFilesAsync(List<string> selectedFiles, string containerName);
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

            //Setting up  CORS - Will allow for streaming of files contained on Azure Blobs
            //Getting client service properties
            var serviceProperties = blobClient.GetServiceProperties();

            serviceProperties.Cors.CorsRules.Clear();

            serviceProperties.Cors.CorsRules.Add(new CorsRule()
            {
                AllowedHeaders = { "*" },
                AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Head | CorsHttpMethods.Post,
                AllowedOrigins = { "*" },
                ExposedHeaders = { "*" },
                MaxAgeInSeconds = 600
            });

            blobClient.SetServiceProperties(serviceProperties);

            //Check if a blob container of that name exists
            var blobContainer = blobClient.GetContainerReference(containerName);

            //Create new container if it does not exist
            await blobContainer.CreateIfNotExistsAsync();

            //Set blob container to public
            BlobContainerPermissions containerPermission = new BlobContainerPermissions();
            containerPermission.PublicAccess = BlobContainerPublicAccessType.Blob; //Set blobs to public
            await blobContainer.SetPermissionsAsync(containerPermission);

            return blobContainer;
        }

        public async Task GetBlobContainer()
        {

        }

        public async Task<string> GetBlobUri(string fileName, string containerName)
        {
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get container
            var blobContainer = blobClient.GetContainerReference(containerName);

            //Get blob
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

            //Get blob Uri
            var blobUri = blob.Uri.ToString();

            return blobUri;
        }

        public async Task UploadAsync(IEnumerable<HttpPostedFileBase> files, string containerName)
        {
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get container
            var blobContainer = blobClient.GetContainerReference(containerName);

            //Iterate and add files to container
            foreach(var file in files)
            {
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);
                //Set content type to match uploaded file
                blob.Properties.ContentType = file.ContentType;
                blob.UploadFromStream(file.InputStream);
                file.InputStream.Close();
            };
        }

        public async Task<Stream> DownloadAsync(string fileName, string containerName)
        {
            MemoryStream stream = new MemoryStream();

            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get container
            var blobContainer = blobClient.GetContainerReference(containerName);

            //Get blob through uri
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

            await blob.DownloadToStreamAsync(stream);            
            stream.Position = 0;

            Stream blobStream = blob.OpenReadAsync().Result;

            //return Blob
            return blobStream;
        }

        public async Task<List<CloudBlockBlob>> DownloadRepositoryAsync(string containerName)
        {
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get container
            var blobContainer = blobClient.GetContainerReference(containerName);          

            //Get list of blobs
            var listOfBlobs = blobContainer.ListBlobs();

            List<CloudBlockBlob> blobList = new List<CloudBlockBlob>();
            
            foreach(var item in listOfBlobs)
            {
                MemoryStream memStream = new MemoryStream();

                CloudBlockBlob blob = (CloudBlockBlob)item;
                blob.FetchAttributes();

                //await blob.DownloadToStreamAsync(memStream);
                //memStream.Position = 0;

                blobList.Add(blob);                
            }
            return blobList;            
        }

        public async Task<List<CloudBlockBlob>> DownloadSpecifiedFilesAsync(List<string> selectedFiles, string containerName)
        {
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get container
            var blobContainer = blobClient.GetContainerReference(containerName);

            //List for retrieved blobs
            List<CloudBlockBlob> blobList = new List<CloudBlockBlob>();

            //Get blobs via items in selected files list
            foreach(var item in selectedFiles)
            {
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(item);
                blob.FetchAttributes();

                //Add blob to list
                blobList.Add(blob);
            }

            return blobList;
        }

        public async Task DeleteAsync(string containerName, string fileUri)
        {
            //Create connection to client
            var connectionStringConfiguration = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;
            var cloudStorageConnection = CloudStorageAccount.Parse(connectionStringConfiguration);
            var blobClient = cloudStorageConnection.CreateCloudBlobClient();

            //Get container
            var blobContainer = blobClient.GetContainerReference(containerName);

            //Convert the string uri from the database into a uri to locate the file
            Uri uri = new Uri(fileUri);

            //Get file name of blob from the Uri path
            string fileName = Path.GetFileName(uri.LocalPath);

            //Get the blob which matches the file name
            var blob = blobContainer.GetBlockBlobReference(fileName);

            //Delete blob if it exists on container
            await blob.DeleteIfExistsAsync();
        }

        public async Task DeleteAllAsync()
        {

        }
    }
}
