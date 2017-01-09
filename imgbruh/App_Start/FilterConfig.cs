using imgbruh.Infrastructure;
using System.Web.Mvc;

namespace imgbruh
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ValidatorActionFilter(), 1);
        }
    }
}
