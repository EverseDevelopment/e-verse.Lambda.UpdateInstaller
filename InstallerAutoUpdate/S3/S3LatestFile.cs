
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
    private readonly string _key;

    public S3LatestFile(string key, string bucketName, string folderPath, string accessKey, string secretKey, string regionEndpoint)
    {
        _bucketName = bucketName;
        _s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(regionEndpoint));
        _key = key;
    }

    public async Task<string> GetLatestFileSignedUrl()
    {
        string signedUrl = GeneratePreSignedURL(_s3Client, _bucketName, _key, TimeSpan.FromMinutes(60)); // URL expires in 60 minutes

        return signedUrl;
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
