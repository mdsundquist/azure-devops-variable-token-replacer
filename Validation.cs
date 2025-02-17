using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;

namespace AzureDevOpsIntegrationTest
{
    internal static class Validation
    {
        internal static Options GetCommandLineOptions(string[] args, out List<string> errors)
        {
            errors = new List<string>();
            ParserResult<Options> parsedOptions = Parser.Default.ParseArguments<Options>(args);
            if (parsedOptions.Value != null)
            {
                if (!string.IsNullOrWhiteSpace(parsedOptions.Value.InputFilePath))
                {
                    try
                    {
                        Path.GetFullPath(parsedOptions.Value.InputFilePath);
                    }
                    catch
                    {
                        errors.Add($"Invalid input file path: {parsedOptions.Value.InputFilePath}");
                    }
                }
                if (!string.IsNullOrWhiteSpace(parsedOptions.Value.OutputFilePath))
                {
                    try
                    {
                        Path.GetFullPath(parsedOptions.Value.OutputFilePath);
                    }
                    catch
                    {
                        errors.Add($"Invalid output file path: {parsedOptions.Value.OutputFilePath}");
                    }
                }
            }

            return parsedOptions.Value;
        }

        internal static bool OptionsAreValid(Settings settings, Options options, out Uri devOpsUri, out string projectName,
            out string openingTag, out string closingTag, ref List<string> errors)
        {
            devOpsUri = null;
            string url = string.IsNullOrWhiteSpace(options?.DevOpsUrl) ? settings.DevOpsUrl : options.DevOpsUrl;
            projectName = string.IsNullOrWhiteSpace(options?.ProjectName) ? settings.ProjectName : options.ProjectName;
            openingTag = string.IsNullOrWhiteSpace(options?.TokenOpeningTag) ? settings.TokenOpeningTag : options.TokenOpeningTag;
            closingTag = string.IsNullOrWhiteSpace(options?.TokenClosingTag) ? settings.TokenClosingTag : options.TokenClosingTag;

            if (errors == null)
            {
                errors = new List<string>();
            }

            if (options == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                errors.Add("Azure DevOps URL is missing; it must be provided as command line argument or in app settings.");
            }
            else if (!Uri.TryCreate(url, UriKind.Absolute, out devOpsUri))
            {
                errors.Add($"Invalid URL: \"{url}\".");
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                errors.Add("Azure DevOps Project Name is missing; it must be provided as command line argument or in app settings.");
            }
            if (string.IsNullOrWhiteSpace(openingTag))
            {
                errors.Add("Replacement token opening tag is missing; it must be provided as command line argument or in app settings.");
            }
            if (string.IsNullOrWhiteSpace(closingTag))
            {
                errors.Add("Replacement token opening tag is missing; it must be provided as command line argument or in app settings.");
            }

            return !errors.Any();
        }
    }
}
