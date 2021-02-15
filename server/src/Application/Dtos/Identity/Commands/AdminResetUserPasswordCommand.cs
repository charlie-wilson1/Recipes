using MediatR;
using Recipes.Application.Contracts.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class AdminResetUserPasswordCommand : IRequest<string>
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<AdminResetUserPasswordCommand, string>
        {
            private readonly IIdentityService _identityService;
            private readonly IPasswordService _passwordService;
            private readonly ICurrentUserService _currentUserService;

            private readonly int DefaultRandomPasswordLength = 8;
            private readonly string EasyToReadChars = "ABCDEFGHKLMNPQRTUVWXYZ2346789!@$%";

            public Handler(
                IIdentityService identityService,
                IPasswordService passwordService, 
                ICurrentUserService currentUserService)
            {
                _identityService = identityService;
                _passwordService = passwordService;
                _currentUserService = currentUserService;
            }

            public async Task<string> Handle(AdminResetUserPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindUserByEmailAsync(request.Email);
                //var adminUsername = _currentUserService.Username;
                //var adminUser = await _identityService.FindUserByUsernameAsync(adminUsername);

                // create and update new password
                var password = string.Empty;
                var passwordIsValid = false;
                
                while (!passwordIsValid)
                {
                    password = GenerateRandomPassword();
                    passwordIsValid = await _passwordService.CheckIfPasswordValid(user, password);
                }

                var token = await _passwordService.GeneratePasswordResetTokenAsync(user);
                await _passwordService.ResetPasswordAsync(user, token, password);

                // TODO: RECORD DATA WHEN LOGGING IS INSERTED INTO PROJECTS

                return password;
            }

            private string GenerateRandomPassword()
            {
                var random = new Random();
                return new string(Enumerable.Repeat(EasyToReadChars, DefaultRandomPasswordLength)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
}
