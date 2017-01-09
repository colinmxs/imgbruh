using System.Threading.Tasks;
using System.Web.Mvc;
using imgbruh.Models;
using MediatR;
using imgbruh.Infrastructure;

namespace imgbruh.Features.Imgs
{
    [imgbruhAuthorize] 
    [RoutePrefix("imgs")]
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

        // GET: Imgs/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        [SignCommand]
        [GenerateCodeName]
        public async Task<ActionResult> Create(Create.Command command)
        {
            await _mediator.SendAsync(command);
            return this.RedirectToActionJson(nameof(Details), new { codeName = command.Name });
        }

        //// GET: Imgs
        //public async Task<ActionResult> Index()
        //{
        //    var imgs = db.Imgs.Include(i => i.User);
        //    return View(await imgs.ToListAsync());
        //}

        // GET: Imgs/Details/5
        [Route("{codename}")]
        public async Task<ActionResult> Details(string codeName)
        {
            var query = new Details.Query
            {
                CodeName = codeName
            };

            var img = await _mediator.SendAsync(query);
            return View(img);
        }



        //// GET: Imgs/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Img img = await db.Imgs.FindAsync(id);
        //    if (img == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Email", img.UserId);
        //    return View(img);
        //}

        //// POST: Imgs/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,CodeName,Url,TimeCreatedUtc,UserId")] Img img)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(img).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "Email", img.UserId);
        //    return View(img);
        //}

        //// GET: Imgs/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Img img = await db.Imgs.FindAsync(id);
        //    if (img == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(img);
        //}

        //// POST: Imgs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Img img = await db.Imgs.FindAsync(id);
        //    db.Imgs.Remove(img);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
    }
}
