using System.Net;
using AkilliPrompt.Domain.Settings;
using AkilliPrompt.Persistence.Services;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using TSID.Creator.NET;

namespace AkilliPrompt.WebApi.Services;

public sealed class R2ObjectStorageManager
{
    private readonly CloudflareR2Settings _cloudflareR2Settings;
    private readonly IAmazonS3 _s3Client;
    private readonly ICurrentUserService _currentUserService;

    public R2ObjectStorageManager(
             IOptions<CloudflareR2Settings> cloudflareR2Settings,
             ICurrentUserService currentUserService)
    {
        _cloudflareR2Settings = cloudflareR2Settings.Value;

        var credentials = new BasicAWSCredentials(
            _cloudflareR2Settings.AccessKey,
            _cloudflareR2Settings.SecretKey);

        _s3Client = new AmazonS3Client(credentials,
            new AmazonS3Config
            {
                ServiceURL = _cloudflareR2Settings.ServiceUrl
            });

        _currentUserService = currentUserService;
    }

    public async Task<string> UploadPromptPicAsync(IFormFile file, long promptId, CancellationToken cancellationToken)
    {
        var fileExtension = Path.GetExtension(file.FileName);

        var key = $"{TsidCreator.GetTsid()}{fileExtension}";

        var request = new PutObjectRequest
        {
            Key = key,
            InputStream = file.OpenReadStream(),
            BucketName = _cloudflareR2Settings.PromptPicsBucketName,
            DisablePayloadSigning = true,
        };

        request.Metadata.Add("promptId", promptId.ToString());
        request.Metadata.Add("userId", _currentUserService.UserId.ToString());

        var response = await _s3Client.PutObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new Exception($"Failed to upload prompt pic. Status code: {response.HttpStatusCode}");

        return key;
    }

    public async Task<string> UploadPromptPicAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var fileExtension = Path.GetExtension(file.FileName);

        var key = $"{TsidCreator.GetTsid()}{fileExtension}";

        var request = new PutObjectRequest
        {
            Key = key,
            InputStream = file.OpenReadStream(),
            BucketName = _cloudflareR2Settings.PromptPicsBucketName,
            DisablePayloadSigning = true,
        };

        request.Metadata.Add("userId", _currentUserService.UserId.ToString());

        var response = await _s3Client.PutObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new Exception($"Failed to upload prompt pic. Status code: {response.HttpStatusCode}");

        return key;
    }

    public async Task RemovePromptPicAsync(string key, CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _cloudflareR2Settings.PromptPicsBucketName,
            Key = key
        };

        var response = await _s3Client.DeleteObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode != HttpStatusCode.NoContent)
            throw new Exception($"Failed to delete prompt pic. Status code: {response.HttpStatusCode}");
    }
}
