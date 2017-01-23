using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace imgbruh.Infrastructure
{
    public class MustRegisterFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var result = new ContentResult();                
                var content = JsonConvert.SerializeObject(new { Error = "Log in to rate." });
                result.Content = content;
                result.ContentType = "application/json";
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = result;
            }
        }
    }
}