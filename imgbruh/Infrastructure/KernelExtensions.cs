using Ninject;
using Ninject.Web.Mvc.Filter;
using Ninject.Web.Mvc.FilterBindingSyntax;
using System;
using System.Web;
using System.Web.Mvc;

namespace imgbruh.Infrastructure
{
    public static class KernelExtensions
    {
        public const string FilterIdMetadataKey = "FilterId";

        public static IFilterBindingInNamedWithOrOnSyntax<TFilter> BindFilterToAttribute<TFilter, TAttribute>(this IKernel kernel, FilterScope scope, int? order, bool isAuthenticated)
        {
            var filterId = Guid.NewGuid();

            var filterBinding = kernel.Bind<TFilter>().ToSelf();
            filterBinding.WithMetadata(FilterIdMetadataKey, filterId);

            var ninjectFilterBinding = kernel.Bind<INinjectFilter>().ToConstructor<NinjectFilter<TFilter>>(
                x => new NinjectFilter<TFilter>(x.Inject<IKernel>(), scope, order, filterId));
            
            var dreturn = new FilterFilterBindingBuilder<TFilter>(ninjectFilterBinding, filterBinding)
                .When((controllerContext, actionDescriptor) => {
                    var attrExists = actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(TAttribute), true).Length != 0;
                    var cbase = new HttpContextWrapper(HttpContext.Current);
                    return (cbase.User.Identity.IsAuthenticated == isAuthenticated) && attrExists;
                });
            return dreturn;
        }
    }
}