using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace InstallerAutoUpdate;

public class Function
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var response = new APIGatewayProxyResponse
        {
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        request.QueryStringParameters.TryGetValue("inputVersion", out string inputVersionString);
        request.QueryStringParameters.TryGetValue("bucketName", out string bucketName);
        request.QueryStringParameters.TryGetValue("folderName", out string folderName);

        var accessKeyId = System.Environment.GetEnvironmentVariable("AWSKey");
        var secretAccessKey = System.Environment.GetEnvironmentVariable("AWSSecret");
        string regionAws = System.Environment.GetEnvironmentVariable("region");

        if (request.Path == "/CurrentVersion")
        {
            
            var s3Helper = new S3Helper(bucketName, folderName, accessKeyId, secretAccessKey, regionAws);
            var latestFileKey = s3Helper.GetLatestFileVersionAsync().GetAwaiter().GetResult();

            int.TryParse(inputVersionString, out int currentVersion);

            if (currentVersion > latestFileKey)
            {
                response.StatusCode = 200;
                response.Body = "true";
            }
            else
            {
                response.StatusCode = 200;
                response.Body = "false";
            }   
        }
        else if (request.Path == "/LatestInstaller")
        {
            var s3LatestFile = new S3LatestFile(bucketName, folderName, accessKeyId, secretAccessKey, regionAws);
            var latestFileSignedUrl = s3LatestFile.GetLatestFileSignedUrl().GetAwaiter().GetResult();

            response.StatusCode = 200;
            response.Body = latestFileSignedUrl;
        }
        else
        {
            response.StatusCode = 404;
            response.Body = "Not Found";
        }

        return response;
    }
}
