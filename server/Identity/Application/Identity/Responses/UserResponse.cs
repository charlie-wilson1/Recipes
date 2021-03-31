using System;
using System.Collections.Generic;

namespace Recipes.Identity.Application.Identity.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByUsername { get; set; }
        public string LastModifiedByUsername { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
