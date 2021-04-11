using System.IO;

namespace Recipes.Core.Application.Common.Models
{
    public class UploadedFileDto
    {
        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
