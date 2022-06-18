using System;
using System.Threading.Tasks;
using DreamCloud.AzureBlob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DreamCloud.Functions.Functions
{
    public class UploadFileToBlobFunction
    {
        private readonly IFileService _fileService;

        public UploadFileToBlobFunction(IFileService fileService)
        {
            _fileService= fileService;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string uploadFileName = "australia-brisbane-city-2.jpg";
            string blobName = $"australia-brisbane-city-2_{DateTime.UtcNow:yy-MM-dd-hh-mm-ss}.jpg";
            
            var blobContentInfo = await _fileService.UploadFileAsync(uploadFileName, blobName);
            log.LogInformation($"uploaded blob ETag: '{blobContentInfo.ETag}'");
        }
    }
}
