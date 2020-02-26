using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Project.Services
{
    interface IAzureBlobService
    {
    }

    public class AzureBlobService : IAzureBlobService
    {
        private string storageConnectionString = ConfigurationManager.ConnectionStrings["StorageClient"].ConnectionString;

        public async Task CreateBlobContainer(string containerName)
        {

        }

        public async Task GetBlobContainer()
        {

        }

        public async Task UploadAsync(IFormFileCollection files)
        {

        }

        public async Task DeleteAsync(string fileUri)
        {

        }

        public async Task DeleteAllAsync()
        {

        }
    }
}
