using System.Threading.Tasks;
using System.Web.Mvc;
using imgbruh.Models;
using MediatR;
using imgbruh.Infrastructure;

namespace imgbruh.Features.Imgs
{
    [ImgbruhAuthorize] 
    [RoutePrefix("")]
    public class ImgsController : UserManagerController
    {
        #region fields
        private ApplicationUser _user;
        private IMediator _mediator;
        #endregion
        #region privates
        private async Task<ApplicationUser> GetUserAsync()
        {
            if (_user == null)
            {
                _user = await UserManager.FindByNameAsync(User.Identity.Name);
                _user.Authenticate(User);
            }
            return _user;
        }
        #endregion
        #region constructors

        public ImgsController(IMediator mediator, ApplicationUserManager userManager) : base(userManager)
        {
            _mediator = mediator;
        }
        #endregion

        [Route("")]
        public ActionResult Create()
        {
            ViewBag.Url = Request.Url;
            return View();
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        [SignCommand]
        [GenerateCodeName]
        public async Task<ActionResult> CreateAsync(Create.Command command)
        {
            await _mediator.SendAsync(command);
            return this.RedirectToActionJson(nameof(DetailsAsync), new { codeName = command.Name });
        }        

        [Route("{codename}")]
        public async Task<ActionResult> DetailsAsync(string codeName)
        {
            var query = new Details.Query
            {
                CodeName = codeName
            };

            var img = await _mediator.SendAsync(query);
            return View(img);
        }
    }
}
