using CommandLine;

namespace AzureDevOpsIntegrationTest
{
    internal class Options
    {
        [Option('u', "url", Required = false,
            HelpText = "URL of Azure DevOps organization. Required unless default set in program configuration.")]
        public string DevOpsUrl { get; set; }

        [Option('p', "project", Required = false,
            HelpText = "Name of Azure DevOps project. Required unless default set in program configuration.")]
        public string ProjectName { get; set; }

        [Option('g', "group", Required = true, HelpText = "Name of Azure DevOps variable group.")]
        public string VariableGroupName { get; set; }

        [Option('i', "input", Required = true, HelpText = "Path to input file.")]
        public string InputFilePath { get; set; }

        [Option('o', "output", Required = true, HelpText = "Path to output file.")]
        public string OutputFilePath { get; set; }

        [Option("openingTag", Required = false,
            HelpText = "Opening tag of replacement token. Required unless default set in program configuration.")]
        public string TokenOpeningTag { get; set; }

        [Option("closingTag", Required = false,
            HelpText = "Closing tag of replacement token. Required unless default set in program configuration.")]
        public string TokenClosingTag { get; set; }
    }
}
