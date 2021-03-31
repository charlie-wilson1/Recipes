using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Application.Identity.Responses;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class RegisterCommand : IRequest<UserWithTokenResponse>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }

        public class Handler : IRequestHandler<RegisterCommand, UserWithTokenResponse>
        {
            private readonly IInvitationRepository _invitationRepository;
            private readonly IInvitationService _invitationService;
            private readonly IUserService _userServcie;

            public Handler(
                IInvitationRepository invitationRepository,
                IInvitationService invitationService,
                IUserService userServcie)
            {
                _invitationRepository = invitationRepository;
                _invitationService = invitationService;
                _userServcie = userServcie;
            }

            public async Task<UserWithTokenResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                DecodeRequest(request);
                await EnsureValidUserData(request.Email, request.Username, request.Token, cancellationToken);
                var response = await _userServcie.Insert(request, cancellationToken);
                await Cleanup(request.Email, cancellationToken);
                return response;
            }

            private static void DecodeRequest(RegisterCommand request)
            {
                request.Token = WebUtility.UrlDecode(request.Token);
                request.Email = request.Email.Contains("@") ? request.Email : WebUtility.UrlDecode(request.Email);
            }

            private async Task EnsureValidUserData(string email, string username, string token, CancellationToken cancellationToken)
            {
                await _userServcie.ValidateNewEmail(email, cancellationToken);
                await _userServcie.ValidateNewUsername(username, cancellationToken);
                await _invitationService.ValidateToken(email, token, cancellationToken);
            }

            public async Task Cleanup(string email, CancellationToken cancellationToken)
            {
                var invitation = await _invitationRepository.GetByEmailAsync(email, cancellationToken);
                await _invitationRepository.DeleteInvitationAsync(invitation.Id, cancellationToken);
            }
        }
    }
}
