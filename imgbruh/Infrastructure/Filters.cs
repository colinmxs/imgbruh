using imgbruh.Models;
using imgbruh.Models.NameGeneration;
using System.Web.Mvc;

namespace imgbruh.Features.Imgs
{
    public class CodeNameCommandFilter : IActionFilter
    {
        private readonly INameGenerator _nameGenerator;
        private readonly ImgbruhContext _context;
        private string _name;

        public CodeNameCommandFilter(INameGenerator nameGenerator, ImgbruhContext context)
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
            command.Name = _name;
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
}