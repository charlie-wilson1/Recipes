using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public string Username { get; set; }

        public DeleteUserCommand(string username)
        {
            Username = username;
        }

        public class Handler : IRequestHandler<DeleteUserCommand>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUserService _userService;

            public Handler(IUserRepository userRepository, IUserService userService)
            {
                _userRepository = userRepository;
                _userService = userService;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetByUsername(request.Username, cancellationToken);
                user.DeleteUser();
                await _userRepository.UpdateAsync(user, cancellationToken);
                return Unit.Value;
            }
        }
    }
}
