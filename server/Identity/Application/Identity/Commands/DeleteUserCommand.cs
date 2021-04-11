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
            private readonly ICurrentUserService _currentUserService;
            private readonly IDateTime _dateTime;

            public Handler(IUserRepository userRepository, IUserService userService, ICurrentUserService currentUserService, IDateTime dateTime)
            {
                _userRepository = userRepository;
                _userService = userService;
                _currentUserService = currentUserService;
                _dateTime = dateTime;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetByUsername(request.Username, cancellationToken);
                user.DeleteUser(_currentUserService.UserId, _dateTime.UtcNow);
                await _userRepository.UpdateAsync(user, cancellationToken);
                return Unit.Value;
            }
        }
    }
}
