using Recipes.Core.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Core.Application.Contracts.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(UploadedFileDto fileDto, CancellationToken cancellationToken);
    }
}
