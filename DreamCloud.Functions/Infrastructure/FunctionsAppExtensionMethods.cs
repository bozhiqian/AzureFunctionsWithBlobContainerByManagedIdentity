using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DreamCloud.Functions.Infrastructure;

public static class FunctionsAppExtensionMethods
{
    public static IConfigurationBuilder AddAppSettingsJson(this IConfigurationBuilder builder, FunctionsHostBuilderContext context)
    {
        builder.AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false);
        builder.AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false);
        return builder;
    }
}