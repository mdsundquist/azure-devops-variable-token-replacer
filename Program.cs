using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;

namespace AzureDevOpsIntegrationTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var errors = new List<string>();

            Settings settings = GetConfiguration();
            
            Options options = Validation.GetCommandLineOptions(args, out errors);
            if (!Validation.OptionsAreValid(settings, options, out Uri devOpsUri, out string projectName,
                out string openingTag, out string closingTag, ref errors) || errors.Any())
            {
                WriteErrors(errors);
                return;
            }

            try
            {
                // Get contents of file whose tokens will be replaced
                string tokenizedFileContents = File.ReadAllText(options.InputFilePath);

                // Create instance of VssConnection using web sign-in modal
                VssConnection connection = new VssConnection(devOpsUri, new VssClientCredentials());

                TaskAgentHttpClient client = connection.GetClient<TaskAgentHttpClient>();

                // Get variable names and values from Azure DevOps project and variable group
                VariableGroup variableGroups = client
                    .GetVariableGroupsAsync(project: projectName, groupName: options.VariableGroupName)
                    .Result.FirstOrDefault();

                IDictionary<string, string> dict = new Dictionary<string, string>(new CaseInsensitiveComparer());

                // Add tokenized variable name and variable value case-insensitive dictionary
                foreach (KeyValuePair<string, VariableValue> variable in variableGroups.Variables)
                {
                    dict.Add(key: $"{openingTag}{variable.Key}{closingTag}", value: variable.Value.Value);
                }

                // Build regex
                string regexPattern = Regex.Escape(openingTag) + @"\b[a-zA-Z0-9]+(?:[\._][a-zA-Z0-9]+)*\b" + Regex.Escape(closingTag);
                Regex regex = new Regex(regexPattern, RegexOptions.Compiled);

                // Replace tokens with values if matches found in variable group, otherwise warn the user and leave token as-is
                string detokenizedFileContents = regex.Replace(tokenizedFileContents, match =>
                    {
                        if (dict.TryGetValue(match.Value, out string value))
                        {
                            return value;
                        }
                        else
                        {
                            Console.WriteLine($"WARNING: No matching variable found for token \"{match.Value}\"");
                            return "";
                        }
                    });

                // Write detokenized contents to output file
                File.WriteAllText(options.OutputFilePath, detokenizedFileContents);

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                WriteErrors(errors);
            }
        }

        private static Settings GetConfiguration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            return configuration.GetRequiredSection("Settings").Get<Settings>();
        }

        private static void WriteErrors(List<string> errors)
        {
            errors.ForEach(error => Console.WriteLine(error));
        }
    }
}
