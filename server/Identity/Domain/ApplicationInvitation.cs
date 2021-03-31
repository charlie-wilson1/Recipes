using Recipes.Identity.Domain.Generic;
using System;

namespace Recipes.Identity.Domain
{
    public class ApplicationInvitation : EntityWithStringId
    {
        public ApplicationInvitation() {}
        public ApplicationInvitation(
            string id,
            string token,
            string email,
            string createdByUserId,
            DateTime createDate)
        {
            Id = id;
            Token = token;
            Email = email;
            CreatedByUserId = createdByUserId;
            CreatedDate = createDate;
        }

        public string Email { get; private set; }
        public string Token { get; private set; }
        public string CreatedByUserId { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public void Create(string email, string token, string createdByUserId, DateTime dateTime)
        {
            Email = email;
            Token = token;
            CreatedByUserId = createdByUserId;
            CreatedDate = dateTime;
        }
    }
}
