using Bogus;
using MediatR;
using Moq;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Contracts.Notifications.SendGrid;
using Recipes.Application.Dtos.Identity.Commands;
using Recipes.Application.Dtos.Notifications.SendGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Recipes.Application.Test.Dtos.Identity.Commands
{
    public class AdminRegisterUserCommandTests
    {
        private readonly Mock<IIdentityService> identityServiceMock;
        private readonly Mock<IEmailService> emailServiceMock;
        private readonly IRequestHandler<AdminRegisterUserCommand> requestHandler;

        public AdminRegisterUserCommandTests()
        {
            identityServiceMock = new Mock<IIdentityService>();
            emailServiceMock = new Mock<IEmailService>();
            requestHandler = new AdminRegisterUserCommand.Handler(identityServiceMock.Object, emailServiceMock.Object);

            Environment.SetEnvironmentVariable("CLIENT_URI", "https://CLIENT_URI");
        }

        [Fact]
        public async Task Handle_CallsIdentityServiceInsertPasswordlessUserAsyncWithEmailProperty()
        {
            var command = new Faker<AdminRegisterUserCommand>()
                .RuleFor(x => x.Email, f => f.Person.Email)
                .Generate();

            await requestHandler.Handle(command, CancellationToken.None);

            identityServiceMock.Verify(x => x.InsertPasswordlessUserAsync(It.Is<string>(y => y == command.Email)), Times.Once);
        }

        [Fact]
        public async Task Handle_CallsEmailServiceSendTemplatedEmailWithProperData()
        {
            var command = new Faker<AdminRegisterUserCommand>()
                .RuleFor(x => x.Email, f => f.Person.Email)
                .Generate();

            var dto = new SendGridTemplatedEmailDto
            {
                SendToEmail = command.Email,
                SendToUsername = command.Email,
                RedirectUri = new Uri($"https://CLIENT_URI/register?email={command.Email}")
            };

            await requestHandler.Handle(command, CancellationToken.None);

            emailServiceMock.Verify(x => x.SendTemplatedEmail(
                It.Is<SendGridTemplatedEmailDto>(y => 
                    y.SendToEmail == dto.SendToEmail &&
                    y.SendToUsername == dto.SendToUsername &&
                    y.RedirectUri == dto.RedirectUri)));
        }
    }
}
