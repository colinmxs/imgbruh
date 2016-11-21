using imgbruh.Models;

namespace imgbruh.Features
{
    public class SignableCommand
    {
        private ApplicationUser _user;

        public void Sign(ApplicationUser user)
        {
            if (user.IsAuthenticated)
            {
                _user = user;
            }
        }
        public ApplicationUser User { get { return _user; } }
    }
}