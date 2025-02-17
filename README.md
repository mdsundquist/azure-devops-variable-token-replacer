 # azure-devops-variable-token-replacer
 A .NET console application used to replace tokens in a file with matching values from an Azure DevOps variable group

#### Command Line Options:
| Option | Description |
|---|---|
| *-u, --url* | URL of Azure DevOps organization. *Required unless default set in appsettings.json*. |
| *-p, --project* | Name of Azure DevOps project. *Required unless default set in appsettings.json*. |
| *-g, --group* | Name of Azure DevOps variable group. *Required*. |
| *-i, --input* | Path to input file. *Required*. |
| *-o, --output* | Path to output file. *Required*. |
| *\-\-openingTag* | Opening tag of replacement token. *Required unless default set in appsettings.json*. |
| *\-\-closingTag* | Closing tag of replacement token. *Required unless default set in appsettings.json*. |
| *\-\-help* | Display help screen. |
| *\-\-version* | Display version information.

#### Example *appsettings.json*: 
```
{
  "Settings": {
    "DevOpsUrl": "https://dev.azure.com/org_name/", //or http://org_name.visualstudio.com/
    "ProjectName": "project_name",
    "TokenOpeningTag": "#{",
    "TokenClosingTag": "}#"
  }
}
```
