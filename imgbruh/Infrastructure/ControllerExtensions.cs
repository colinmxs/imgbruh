using Newtonsoft.Json;
using System.Web.Mvc;

namespace imgbruh.Infrastructure
{
    public static class ControllerExtensions
    {
        const string ApplicationJson = "application/json";
        public static ActionResult RedirectToActionJson<TController>(this TController controller, string action, object routeValues)
            where TController : Controller
        {
            return controller.JsonNet(new { redirect = controller.Url.Action(action, routeValues) });
        }

        public static ContentResult JsonNet(this Controller controller, object model)
        {
            var serialized = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new ContentResult
            {
                Content = serialized,
                ContentType = ApplicationJson
            };
        }
    }
}