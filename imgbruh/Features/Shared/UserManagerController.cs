using System.Web.Mvc;

namespace imgbruh.Features
{
    public class UserManagerController : Controller
    {
        public UserManagerController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        private ApplicationUserManager _userManager;
            

        internal ApplicationUserManager UserManager
        {
            get
            {
                return _userManager;
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}