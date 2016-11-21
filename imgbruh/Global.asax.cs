using FluentValidation.Mvc;
using imgbruh.Infrastructure;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace imgbruh
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FluentValidationModelValidatorProvider.Configure();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FeatureViewLocationRazorViewEngine());
        }
    }
}
