using Microsoft.AspNetCore.Identity;
using Recipes.Application.Dtos.Identity.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Identity.Services
{
    public static class IdentityMappers
    {
        public static List<string> MapUserRolesFromUserDictionary(this IdentityUser user, Dictionary<string, IList<IdentityUser>> userLists)
        {
            var roles = new List<string>();

            foreach (var userList in userLists)
            {
                if (userList.Value.Contains(user))
                {
                    roles.Add(userList.Key);
                }
            }

            return roles;
        }
    }
}
