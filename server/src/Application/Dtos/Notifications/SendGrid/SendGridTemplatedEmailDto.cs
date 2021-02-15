using System;

namespace Recipes.Application.Dtos.Notifications.SendGrid
{
    public class SendGridTemplatedEmailDto
    {
        public string SendToEmail { get; set; }
        public string SendToUsername { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
