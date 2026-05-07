using Azure.Storage.Blobs;

namespace Delivery.Api.Service
{
    public class BlobService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("orders-images");
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            // Ensures the container exists before the first upload
            await _blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = _blobContainerClient.GetBlobClient(file.FileName);

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }
    }
}
