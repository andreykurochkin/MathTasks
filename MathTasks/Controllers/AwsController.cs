using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;

namespace MathTasks.Controllers
{

    public class AwsFileInfo
    {
        public string Name { get; set; }
    }

    public class AwsFiles
    {
        private IEnumerable<AwsFileInfo> _innerList;

        public AwsFiles(IEnumerable<AwsFileInfo> innerList)
        {
            _innerList = innerList;
        }

        //public static string ToJson()
        //{
        //    JsonSerializer.Serialize()
        //}
    }

    public class AwsController : Controller
    {

        public async Task<string> Index()
        {
            var s3Objects = (await GetS3Objects()).S3Objects;
            var result = JsonSerializer.Serialize(s3Objects);
            return result;
        }

        private readonly IAmazonS3 _s3Client;
        private const string BucketName = "mathtasksbucket";

        public AwsController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public Task<ListObjectsV2Response> GetS3Objects()
        {
            var request = new ListObjectsV2Request { BucketName = BucketName };
            return _s3Client.ListObjectsV2Async(request);
        }

        [HttpPost]
        public async Task<HttpStatusCode> CreateFolder(string newFolderName, string bucketName = BucketName, string prefix = "")
        {
            var putObjectRequest = new PutObjectRequest();
            putObjectRequest.BucketName = bucketName;
            putObjectRequest.Key = (prefix.TrimEnd('/') + "/" + newFolderName.TrimEnd('/') + "/").TrimStart('/');
            var response = await _s3Client.PutObjectAsync(putObjectRequest);
            return response.HttpStatusCode;
        }

        private static string CreateFolderName(string folderName, string prefix) =>
            (prefix.TrimEnd('/') + "/" + folderName.TrimEnd('/') + "/").TrimStart('/');

        private static string CreateFileName(string path, string prefix) =>
            (prefix.TrimEnd('/') + "/" + path.TrimEnd('/')).TrimStart('/');

        [HttpPost]
        public async Task<HttpStatusCode> UploadFile(string path, string bucketName = BucketName, string prefix = "")
        {
            using var fileStream = new FileInfo(path).OpenRead();
            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = CreateFileName(Path.GetFileName(path), prefix: prefix),
                InputStream = fileStream
            };
            var response = await _s3Client.PutObjectAsync(putObjectRequest);
            return response.HttpStatusCode;
        }
    }
}
