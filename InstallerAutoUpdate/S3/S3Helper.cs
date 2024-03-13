using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using InstallerAutoUpdate;

public class S3Helper
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;
    private readonly string _folderPath;

    public S3Helper(string bucketName, string folderPath, string accessKey, string secretKey, string regionEndpoint)
    {
        _bucketName = bucketName;
        _folderPath = folderPath + "/";
        _s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(regionEndpoint));
    }

    public async Task<InstallerAutoUpdate.FileInfo> GetLatestFileVersionAsync()
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

        var latestFile = files.Select(file => new InstallerAutoUpdate.FileInfo
        {
            Key = file.Key,
            Version = ExtractVersionFromFileName(file.Key),
            NumberVersion = ConverVersionToNumbers(ExtractVersionFromFileName(file.Key))
        })
        .OrderByDescending(file => file.Version)
        .FirstOrDefault();

        return latestFile;
    }

    private string ExtractVersionFromFileName(string fileName)
    {
        if (fileName == _folderPath)
        {
            return "0";
        }

        fileName = fileName.Substring(_folderPath.Length);
        string nameNoPath = Path.GetFileNameWithoutExtension(fileName);


        return nameNoPath;
    }

    private int ConverVersionToNumbers(string versionNumber)
    {
        string nameNoCharacters = Regex.Replace(versionNumber, "[^0-9]", "");
        int result = int.Parse(nameNoCharacters);

        return result;
    }
}
