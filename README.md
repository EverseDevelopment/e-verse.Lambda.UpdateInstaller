# e-verse.Lambda.UpdateInstaller

<p align="center">
<img src="https://s3.amazonaws.com/everse.assets/GithubReadme/APIAutoInstaller-01.jpg" />
</p>


<h3 align="left">This is a C# code for AWS lambda to check an S3 for the latest installer files
<br/>




## Getting Started
This is a C# code, you can just run it locally, just make sure you have the followin environment variables:

"AWSKey": The Key of an IAM user with access to S3
"AWSSecret": The Secret of an IAM user with access to S3 
"region": The region where the S3 will be located

Also when deploying from Visual studio to AWS Lambda make sure to add a file named "aws-lambda-tools-defaults.json" inside the InstallerAutoUpdate folder with the following data:

```json
{
    "Information" : [
        "This file provides default values for the deployment wizard inside Visual Studio and the AWS Lambda commands added to the .NET Core CLI.",
        "To learn more about the Lambda commands with the .NET Core CLI execute the following command at the command line in the project root directory.",
        "dotnet lambda help",
        "All the command line options for the Lambda command can be specified in this file."
    ],
    "profile"     : "",
    "region"      : "",
    "configuration" : "",
    "function-runtime" : "",
    "function-memory-size" : ,
    "function-timeout"     : ,
    "function-handler"     : "",
    "framework"            : "",
    "function-name"        : "",
    "package-type"         : "",
    "function-role"        : "",
    "function-architecture" : "",
    "function-subnets"      : "",
    "function-security-groups" : "",
    "tracing-mode"             : "",
    "environment-variables"    : "\"AWSSecret\"=\"YourAmazonSecret\";\"AWSKey\"=\"YourAmazonKey\";\"region\"=\"YourAmazonRegion\"",
    "image-tag"                : "",
    "function-description"     : ""
}
```

## Contributors
This repo is primarily managed by [E-verse](https://www.e-verse.co/) and by [People Like You™](https://github.com/EverseDevelopment/e-verse.Lambda.UpdateInstaller/pulse).

## Help improve this repo
If you're interested in contributing to this repo, just submit a [pull request](https://github.com/EverseDevelopment/e-verse.Lambda.UpdateInstaller/pulls) or a [feature request](https://github.com/EverseDevelopment/e-verse.Lambda.UpdateInstaller/issues) .

## About us ##

We are an international mix of AEC professionals, product designers, and software developers. We work together to transform construction requirements into accurate and partnership-driven technological solutions.

<p align="center" width="100%">
    <a href="https://www.e-verse.com/">
    <img src="https://github.com/EverseDevelopment/DynaForge/blob/main/Assets/e-verse_logo_no%20slogan.jpg" width="732" height="271" align="center">
    </a>
</p>
