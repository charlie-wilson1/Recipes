using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Identity.Models
{
    public class IdentitySeedSettings
    {
        public string AdminEmail { get; set; }
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
    }
}
