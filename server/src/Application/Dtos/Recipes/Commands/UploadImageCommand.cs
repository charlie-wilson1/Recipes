using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Recipes.Application.Contracts;
using Recipes.Application.Dtos.Recipes.Responses;
using Recipes.Domain.Entities.Recipes;

namespace Recipes.Application.Dtos.Recipes.Commands
{
    public class UploadImageCommand : IRequest<ImageDto>
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public class Handler : IRequestHandler<UploadImageCommand, ImageDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ImageDto> Handle(UploadImageCommand request, CancellationToken cancellationToken)
            {
                var image = new RecipeImage { 
                    Name = request.Name,
                    Url = request.Url
                };

                var response = _context.RecipeImages.Add(image);
                await _context.SaveChangesAsync();
                return _mapper.Map<ImageDto>(response.Entity);
            }
        }
    }
}
