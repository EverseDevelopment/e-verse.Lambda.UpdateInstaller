
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

public class S3LatestFile
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;
    private readonly string _folderPath;

    public S3LatestFile(string bucketName, string folderPath, string accessKey, string secretKey, string regionEndpoint)
    {
        _bucketName = bucketName;
        _folderPath = folderPath + "/";
        _s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(regionEndpoint));
    }

    public async Task<string> GetLatestFileSignedUrl()
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = _folderPath
        };

        ListObjectsV2Response response;
        var files = new List<S3Object>();

        do
        {
            response = await _s3Client.ListObjectsV2Async(request);
            files.AddRange(response.S3Objects);
            request.ContinuationToken = response.NextContinuationToken;
        }
        while (response.IsTruncated);

        var latestFile = files.Select(file => new
        {
            Key = file.Key,
            Version = ExtractVersionFromFileName(file.Key)
        })
        .OrderByDescending(file => file.Version)
        .FirstOrDefault();

        string signedUrl = GeneratePreSignedURL(_s3Client, _bucketName, latestFile.Key, TimeSpan.FromMinutes(60)); // URL expires in 60 minutes

        return signedUrl;
    }

    private int ExtractVersionFromFileName(string fileName)
    {
        if (fileName == _folderPath)
        {
            return 0;
        }

        fileName = fileName.Substring(_folderPath.Length);
        string nameNoPath = Path.GetFileNameWithoutExtension(fileName);
        string nameNoCharacters = Regex.Replace(nameNoPath, "[^0-9]", "");

        int result = int.Parse(nameNoCharacters);

        return result;
    }

    public static string GeneratePreSignedURL(IAmazonS3 s3Client, string bucketName, string objectKey, TimeSpan expiryDuration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            Expires = DateTime.UtcNow.Add(expiryDuration)
        };

        return s3Client.GetPreSignedURL(request);
    }
}
