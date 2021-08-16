using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using Photos.Models;
namespace Photos
{
    public static class PhotosStorage
    {
        [FunctionName("PhotosStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Blob("photos", FileAccess.ReadWrite,Connection = Literals.StorageConnectionString)] CloudBlobContainer blobContainer,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function photo storage request");
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<PhotoUploadModel>(body);
            var photoId = Guid.NewGuid();
            var photoName = $"{photoId}.jpg";
            await blobContainer.CreateIfNotExistsAsync();
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(photoName);
            var photoBytes = Convert.FromBase64String(request.Photo);
            await cloudBlockBlob.UploadFromByteArrayAsync(photoBytes,0,photoBytes.Length);
            logger?.LogInformation($"Successfully uploaded photo = {photoId}.jpg");
            return new OkObjectResult(photoId);

        }
    }
}
