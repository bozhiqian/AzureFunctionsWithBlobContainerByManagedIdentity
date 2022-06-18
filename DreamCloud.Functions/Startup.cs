using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using Azure.Storage.Blobs;
using DreamCloud.AzureBlob;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DreamCloud.Functions.Infrastructure;

[assembly: FunctionsStartup(typeof(DreamCloud.Functions.Startup))]
namespace DreamCloud.Functions;

public class Startup : FunctionsStartup
{
    private IConfiguration _configuration;
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var storageAccountName = _configuration["StorageAccountName"];
        var blobContainerName = _configuration["BlobContainerName"];

        if (storageAccountName == null)
        {
            // todo... logging.
            throw new ArgumentNullException("StorageAccountName cannot be null!");
        }

        if (blobContainerName == null)
        {
            // todo... logging.
            throw new ArgumentNullException("BlobContainerName cannot be null!");
        }

        builder.Services.AddAzureClients(cfg =>
        {
            // Create a BlobServiceClient that will authenticate through Active Directory
            Uri accountUri = new Uri($"https://{storageAccountName}.blob.core.windows.net/");

            cfg.AddBlobServiceClient(accountUri).WithCredential(new Azure.Identity.DefaultAzureCredential());
        });

        builder.Services.AddSingleton<IFileService>(sp =>
        {
            var blobServiceClient = sp.GetRequiredService<BlobServiceClient>();
            return new FileService(blobServiceClient, blobContainerName);
        });
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        // local.settings.json are automatically loaded when debugging.
        // When running on Azure, values are loaded defined in app settings.
        // See: https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings
        _configuration = builder.ConfigurationBuilder
            .AddAppSettingsJson(builder.GetContext())
            .Build();

        base.ConfigureAppConfiguration(builder);
    }
}