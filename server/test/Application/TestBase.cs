using Bogus;
using Microsoft.AspNetCore.Http;
using Moq;
using Recipes.Application.Contracts.Identity;
using Recipes.WebApi;
using System;
using System.Linq;
using System.Security.Claims;

namespace Recipes.Application.Test
{
    public abstract class TestBase
    {
        public readonly ICurrentUserService _currentUserService;

        protected TestBase()
        {
            var user = new Faker<CurrentUserService>()
                .RuleFor(x => x.Token, f => Guid.NewGuid().ToString())
                .RuleFor(x => x.UserId, f => Guid.NewGuid().ToString())
                .RuleFor(x => x.Username, f => f.Person.UserName)
                .Generate();

            var contextMock = new Mock<IHttpContextAccessor>();

            contextMock.Setup(x => x.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).Returns(user.UserId);

            contextMock.Setup(x => x.HttpContext.User.FindFirstValue(ClaimTypes.Name)).Returns(user.Username);

            contextMock.Setup(x => x.HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split().Last()).Returns(user.Token);

            _currentUserService = new CurrentUserService(contextMock.Object);
        }
    }
}
