using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using imgbruh.Infrastructure;

namespace imgbruh.Features.Imgs
{
    [ImgbruhAuthorize] 
    [RoutePrefix("")]
    public class ImgsController : Controller
    {
        #region fields
        private readonly IMediator _mediator;
        #endregion
        #region constructors

        public ImgsController(IMediator mediator)
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
