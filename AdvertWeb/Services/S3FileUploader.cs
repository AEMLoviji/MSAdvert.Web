using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

namespace AdvertWeb.Services;

public class S3FileUploader : IFileUploader
{
    private const string ConfigurationKey = "ImageBucket";

    private readonly IConfiguration _configuration;

    public S3FileUploader(IConfiguration configuration) => _configuration = configuration;

    public async Task<bool> UploadFileAsync(string fileName, Stream storageStream)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name must be specified.");
        }

        var bucketName = _configuration.GetValue<string>(ConfigurationKey);

        if (storageStream.Length > 0 && storageStream.CanSeek)
        {
            storageStream.Seek(0, SeekOrigin.Begin);
        }

        PutObjectRequest request = new()
        {
            AutoCloseStream = true,
            BucketName = bucketName,
            InputStream = storageStream,
            Key = fileName
        };

        using AmazonS3Client client = new();
        var response = await client.PutObjectAsync(request);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}
