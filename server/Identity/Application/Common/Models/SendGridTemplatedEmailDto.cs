using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Common.Models
{
    public class SendGridTemplatedEmailDto
    {
        public string SendToEmail { get; set; }
        public string SendToUsername { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
