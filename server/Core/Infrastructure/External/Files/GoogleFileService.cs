using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Recipes.Core.Application.Common.Models;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Infrastructure.Loaders.SettingsModels;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Infrastructure.External.Files
{
    public class GoogleFileService : IFileService
    {
        private readonly StorageClient _storageClient;
        private readonly GoogleClientSettings _googleClientSettings;

        public GoogleFileService(IOptions<GoogleClientSettings> googleClientSettings)
        {
            _googleClientSettings = googleClientSettings.Value;
            var filePath = _googleClientSettings.CredentialsFilePath;
            var credential = GoogleCredential.FromFile(filePath);
            _storageClient = StorageClient.Create(credential);
        }

        public async Task<string> UploadFileAsync(UploadedFileDto fileDto, CancellationToken cancellationToken)
        {
            var mediaLink = string.Empty;

            using (var ms = new MemoryStream(fileDto.FileBytes))
            {
                var storageObject = await _storageClient.UploadObjectAsync(
                    _googleClientSettings.BucketName,
                    fileDto.FileName,
                    fileDto.FileType,
                    ms,
                    cancellationToken: cancellationToken);

                mediaLink = storageObject.MediaLink;
            }
            
            return mediaLink;
        }
    }
}
