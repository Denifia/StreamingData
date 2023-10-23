using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs.Models;
using Azure.Storage;

namespace StreamingData.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly BlobContainerClient _containerClient;

        public FileUploadController(BlobServiceClient blobServiceClient)
        {
            _containerClient = blobServiceClient.GetBlobContainerClient("temp");
            _containerClient.CreateIfNotExists();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            var blobClient = _containerClient.GetBlockBlobClient("25mb.txt");

            var transferOptions = new StorageTransferOptions
            {
                // Set the maximum number of parallel transfer workers
                MaximumConcurrency = 5,

                // Set the initial transfer length to 8 MiB
                InitialTransferSize = 8 * 1024 * 1024,

                // Set the maximum length of a transfer to 4 MiB
                MaximumTransferSize = 4 * 1024 * 1024
            };

            var uploadOptions = new BlobUploadOptions()
            {
                TransferOptions = transferOptions
            };

            await blobClient.UploadAsync(HttpContext.Request.Body, uploadOptions);
            return new OkResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return new OkResult();
        }
    }
}