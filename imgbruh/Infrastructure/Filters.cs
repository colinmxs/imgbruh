using imgbruh.Models;
using imgbruh.Models.NameGeneration;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Web.Mvc;
using imgbruh.Infrastructure;
using System.Web.Mvc.Filters;
using System.Linq;

namespace imgbruh.Features.Imgs
{
    public class CreateApplicationUserFilter : IAuthenticationFilter
    {
        private readonly INameGenerator _nameGenerator;
        private readonly imgbruhContext _context;
        public CreateApplicationUserFilter(INameGenerator nameGenerator, imgbruhContext context)
        {
            _nameGenerator = nameGenerator;
            _context = context;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var reservedNames = _context.Users.Select(u => u.UserName).ToArray();
            var user = ApplicationUser.QuickCreate(_nameGenerator.Generate());
            filterContext.HttpContext.SetApplicationUser(user);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
    public class SetApplicationUserFilter : IAuthenticationFilter
    {
        private readonly imgbruhContext _dbContext;
        public SetApplicationUserFilter(imgbruhContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var userName = filterContext.HttpContext.User.Identity.Name;
            var user = _dbContext.Users.SingleOrDefault(u => u.UserName == userName);
            filterContext.HttpContext.SetApplicationUser(user);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }        
    }
    public class AutoRegistrationFilter : IAuthorizationFilter
    {
        private readonly imgbruhContext _dbContext;

        public AutoRegistrationFilter(imgbruhContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.GetApplicationUser();
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }
    public class AutoSignInFilter : IAuthorizationFilter
    {
        private readonly ApplicationSignInManager _signInManager;

        public AutoSignInFilter(ApplicationSignInManager signInManager)
            {
                _signInManager = signInManager;
            }

        public void OnAuthorization(AuthorizationContext filterContext)
            {
                var user = filterContext.HttpContext.GetApplicationUser();
                var claimsIdentity = _signInManager.CreateUserIdentity(user);
                filterContext.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
                _signInManager.SignIn(user, true, true);
            }
    }
    public class AuthenticateApplicationUserFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.GetApplicationUser();
            var principal = filterContext.HttpContext.User;
            user.Authenticate(principal);
            filterContext.HttpContext.SetApplicationUser(user);
        }
    }
    public class CodeNameCommandFilter : IActionFilter
    {
        private readonly INameGenerator _nameGenerator;
        private readonly imgbruhContext _context;
        private string _name;

        public CodeNameCommandFilter(INameGenerator nameGenerator, imgbruhContext context)
        {
            _nameGenerator = nameGenerator;
            _context = context;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var command = (Create.Command)filterContext.ActionParameters["command"];
            _name = _nameGenerator.Generate();
            var padding = Guid.NewGuid().ToString().Substring(0, 4);
            command.Name = _name + padding;
        }
    }
    public class CodeNameRedirectFilter : IActionFilter
    {
        private Create.Command _command;

        public CodeNameRedirectFilter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Result = new RedirectResult("imgs/"+_command.Name);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _command = (Create.Command)filterContext.ActionParameters["command"];
        }
    }
    public class SignCommandFilter : IActionFilter
    {
        public SignCommandFilter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var command = filterContext.ActionParameters["command"];
            var signableCommand = (SignableCommand)command;
            var user = filterContext.HttpContext.GetApplicationUser();
            signableCommand.Sign(user);
            filterContext.ActionParameters["command"] = signableCommand;
        }
    }
}
