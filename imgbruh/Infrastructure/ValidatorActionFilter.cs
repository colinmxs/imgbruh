using System;
using System.Net;
using System.Web.Mvc;

namespace imgbruh.Infrastructure
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.HttpMethod == "GET")
                {
                    var result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    filterContext.Result = result;
                }
                else
                {
                    throw new Exception("That object is straight wacked, yo!");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}