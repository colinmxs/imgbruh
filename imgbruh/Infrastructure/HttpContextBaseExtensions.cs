using imgbruh.Models;
using System.Web;

namespace imgbruh.Infrastructure
{
    public static class HttpContextBaseExtensions
    {
        public static ApplicationUser GetApplicationUser(this HttpContextBase context)
        {
            return context.Items["user"] as ApplicationUser;
        }
        public static void SetApplicationUser(this HttpContextBase context, ApplicationUser user)
        {
            context.Items["user"] = user;
        }
    }
}