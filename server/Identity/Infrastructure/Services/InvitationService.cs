using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Infrastructure.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public InvitationService(
            IInvitationRepository invitationRepository,
            IEmailService emailService,
            ICurrentUserService currentUserService,
            IDateTime dateTime)
        {
            _invitationRepository = invitationRepository;
            _emailService = emailService;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public async Task ValidateNewEmail(string email, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByEmailAsync(email, cancellationToken);

            if (invitation != null)
            {
                throw new ValidationException("Email already exists.");
            }
        }

        public async Task ValidateToken(string email, string token, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByEmailAsync(email, cancellationToken);

            if (invitation == null || invitation.Token != token)
            {
                throw new ValidationException("Invitation doesn't exist.");
            }
        }

        public ApplicationInvitation Create(string email, string token)
        {
            var invitation = new ApplicationInvitation();
            invitation.Create(email, token, _currentUserService.UserId, _dateTime.UtcNow);
            return invitation;
        }

        public async Task SendInvitationEmail(string email, string token, CancellationToken cancellationToken)
        {
            var dto = _emailService.CreateTokenizedEmailDto(email, token, "register");
            await _emailService.SendInvitationEmail(dto, cancellationToken);
        }
    }
}
