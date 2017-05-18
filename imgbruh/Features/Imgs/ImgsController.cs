using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using imgbruh.Infrastructure;
using CodenameGenerator;

namespace imgbruh.Features.Imgs
{
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
        public async Task<ActionResult> CreateAsync(Create.Command command)
        {
            var name = await _mediator.SendAsync(command);
            return this.RedirectToActionJson(nameof(DetailsAsync), new { codeName = name });
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
