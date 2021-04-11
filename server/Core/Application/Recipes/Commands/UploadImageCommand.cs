using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Core.Application.Common.Models;
using Recipes.Core.Application.Contracts.Services;

namespace Recipes.Core.Application.Recipes.Commands
{
    public class UploadImageCommand : IRequest<string>
    {
        public UploadedFileDto Image { get; set; }

        public UploadImageCommand(UploadedFileDto image)
        {
            Image = image;
        }

        public class Handler : IRequestHandler<UploadImageCommand, string>
        {
            private readonly IFileService _fileService;

            public Handler(IFileService fileService)
            {
                _fileService = fileService;
            }

            public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
            {
                return await _fileService.UploadFileAsync(request.Image, cancellationToken);
            }
        }
    }
}
