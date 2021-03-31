using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class CreateInvitationCommand : IRequest
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<CreateInvitationCommand>
        {
            private readonly IUserService _userServcie;
            private readonly IInvitationRepository _invitationRepository;
            private readonly IInvitationService _invitationService;
            private readonly IJwtService _jwtService;

            public Handler(
                IUserService userServcie,
                IInvitationRepository invitationRepository,
                IInvitationService invitationService,
                IJwtService jwtService)
            {
                _userServcie = userServcie;
                _invitationRepository = invitationRepository;
                _invitationService = invitationService;
                _jwtService = jwtService;
            }

            public async Task<Unit> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
            {
                await ValidateEmail(request.Email, cancellationToken);
                var token = _jwtService.GenerateRandomToken();
                await CreateInvitation(request, token, cancellationToken);
                await _invitationService.SendInvitationEmail(request.Email, token, cancellationToken);
                return Unit.Value;
            }

            private async Task CreateInvitation(CreateInvitationCommand request, string token, CancellationToken cancellationToken)
            {
                var invitation = _invitationService.Create(request.Email, token);
                await _invitationRepository.InsertAsync(invitation, cancellationToken);
            }

            public async Task ValidateEmail(string email, CancellationToken cancellationToken)
            {
                await _userServcie.ValidateNewEmail(email, cancellationToken);
                await _invitationService.ValidateNewEmail(email, cancellationToken);
            }
        }
    }
}
