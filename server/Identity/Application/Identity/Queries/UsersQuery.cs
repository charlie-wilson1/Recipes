using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Identity.Responses;

namespace Recipes.Identity.Application.Identity.Queries
{
    public class UsersQuery : IRequest<List<UserResponse>>
    {
        public string SearchQuery { get; set; }

        public class Handler : IRequestHandler<UsersQuery, List<UserResponse>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public Handler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<List<UserResponse>> Handle(UsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _userRepository.GetActiveAsync(request.SearchQuery, cancellationToken);
                return _mapper.Map<List<UserResponse>>(users);
            }
        }
    }
}
