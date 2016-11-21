using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Collections.Generic;
using System;

namespace imgbruh.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private ApplicationUser() { }
        private bool _isAuthenticated = false;
        public bool IsAuthenticated { get { return _isAuthenticated; } }
        public ICollection<Rating> Ratings { get; private set; }
        public ICollection<Comment> Comments { get; private set; }
        public ICollection<Img> Imgs { get; private set; }
        public void Authenticate(IPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated && principal.Identity.GetUserId() == this.Id)
            {
                _isAuthenticated = true;
            }
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here            
            return userIdentity;
        }
        public static ApplicationUser QuickCreate(string userName)
        {
            var user = new ApplicationUser()
            {
                UserName = userName,
                Id = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            return user;
        }            
    }

    public abstract class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }            
    }

    public class UserStore : UserStore<ApplicationUser>
    {
        public UserStore(imgbruhContext context) : base(context)
        {
        }
    }   
}