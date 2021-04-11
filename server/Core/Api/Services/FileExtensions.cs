using Microsoft.AspNetCore.Http;
using Recipes.Core.Application.Common.Models;
using System.IO;
using System.Threading.Tasks;

namespace Recipes.Core.Api.Services
{
    public static class FileExtensions
    {
        public static async Task<UploadedFileDto> ToUploadedFileDto(this IFormFile file)
        {
            if (file.Length == 0)
            {
                return null;
            }

            var fileBytes = new byte[] { };

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            return new UploadedFileDto
            {
                FileName = file.FileName,
                FileType = file.ContentType,
                FileBytes = fileBytes
            };
        }
    }
}
