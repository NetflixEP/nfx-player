using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace nfx_player.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BucketsController(IAmazonS3 s3Client) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBucketAsync(string bucketName)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
        await s3Client.PutBucketAsync(bucketName);
        return Created("buckets", $"Bucket {bucketName} created.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBucketAsync()
    {
        var data = await s3Client.ListBucketsAsync();
        var buckets = data.Buckets.Select(b => b.BucketName);
        return Ok(buckets);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBucketAsync(string bucketName)
    {
        await s3Client.DeleteBucketAsync(bucketName);
        return NoContent();
    }
}