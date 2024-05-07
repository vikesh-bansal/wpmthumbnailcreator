using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ThumbnailCreator
{
    public class ThumbnailCreator
    {
        [FunctionName(nameof(ThumbnailCreator))]
        
        public void Run([BlobTrigger("photos/{name}", Connection = "WpmStorage")]Stream myBlob, [Blob("thumbnails/{name}", FileAccess.Write, Connection = "WpmStorage")] Stream thumbnail, string name, ILogger log)
        {

            Resize(myBlob, thumbnail);
            log.LogInformation($"[{name}] processed.");
        }
        private void Resize(Stream inputStream, Stream outputStream)
        {
            using Image image = Image.Load(inputStream);
            image.Mutate(x => x.Resize(50, 0));
            image.Save(outputStream, JpegFormat.Instance);
        }
    }
}
