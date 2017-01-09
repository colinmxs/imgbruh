[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(imgbruh.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(imgbruh.App_Start.NinjectWebCommon), "Stop")]

namespace imgbruh.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Models;
    using Models.NameGeneration;
    using Ninject.Components;
    using Ninject.Planning.Bindings.Resolvers;
    using System.Collections.Generic;
    using Ninject.Planning.Bindings;
    using Ninject.Infrastructure;
    using Ninject.Extensions.Conventions;
    using System.Linq;
    using System.Reflection;
    using MediatR;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Features;
    using Features.Imgs;
    using Infrastructure;
    using Ninject.Web.Mvc.FilterBindingSyntax;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //bind owin middleware            
            kernel.Bind<IOwinContext>().ToMethod((context) =>
            {
                var cbase = new HttpContextWrapper(HttpContext.Current);
                var owinContext = cbase.GetOwinContext();
                return owinContext;
            });

            kernel.Bind<IAuthenticationManager>().ToMethod((context) =>
            {
                var cbase = new HttpContextWrapper(HttpContext.Current);
                var owinContext = cbase.GetOwinContext();
                return owinContext.Authentication;
            });

            //mediatr bindings
            kernel.Components.Add<IBindingResolver, ContravariantBindingResolver>();
            kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>().SelectAllClasses().BindDefaultInterface());
            kernel.Bind(scan => scan.FromAssemblyContaining<Create>().SelectAllClasses().InNamespaceOf<Create>().BindAllInterfaces());
            kernel.Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
            kernel.Bind<MultiInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.GetAll(t));

            //bind filters
            kernel.BindFilterToAttribute<CreateApplicationUserFilter, imgbruhAuthorizeAttribute>(System.Web.Mvc.FilterScope.Controller, 2, false).WithPropertyValue("SkipOnInvalidModelState", true);
            kernel.BindFilterToAttribute<SetApplicationUserFilter, imgbruhAuthorizeAttribute>(System.Web.Mvc.FilterScope.Controller, 3, true);
            kernel.BindFilterToAttribute<AutoRegistrationFilter, imgbruhAuthorizeAttribute>(System.Web.Mvc.FilterScope.Action, 5, false);
            kernel.BindFilterToAttribute<AutoSignInFilter, imgbruhAuthorizeAttribute>(System.Web.Mvc.FilterScope.Action, 10, false);
            kernel.BindFilter<AuthenticateApplicationUserFilter>(System.Web.Mvc.FilterScope.Action, 90).WhenActionMethodHas<SignCommandAttribute>();
            kernel.BindFilter<SignCommandFilter>(System.Web.Mvc.FilterScope.Action, 100).WhenActionMethodHas<SignCommandAttribute>();
            kernel.BindFilter<CodeNameCommandFilter>(System.Web.Mvc.FilterScope.Action, 100).WhenActionMethodHas<GenerateCodeNameAttribute>();
            kernel.BindFilter<CodeNameRedirectFilter>(System.Web.Mvc.FilterScope.Action, 150).WhenActionMethodHas<RedirectToCodeNameAttribute>();

            //bind EF contexts
            kernel.Bind<imgbruhContext>().ToSelf().InRequestScope();

            //bind identity stuff
            kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore>();
            kernel.Bind<UserManager<ApplicationUser>>().ToSelf();
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            kernel.Bind<ApplicationSignInManager>().ToSelf();
            kernel.Bind<ApplicationUserManager>().ToSelf();

            //bind classes
            kernel.Bind<INameGenerator>().To<UserNameGenerator>().WhenInjectedExactlyInto(typeof(CreateApplicationUserFilter)).InRequestScope();
            kernel.Bind<INameGenerator>().To<UserNameGenerator>().WhenInjectedExactlyInto(typeof(AutoRegistrationFilter)).InRequestScope();
            kernel.Bind<INameGenerator>().To<CodeNameGenerator>().WhenInjectedExactlyInto(typeof(CodeNameCommandFilter)).InRequestScope();            
        }        
    }

    public class ContravariantBindingResolver : NinjectComponent, IBindingResolver
    {
        /// <summary>
        /// Returns any bindings from the specified collection that match the specified service.
        /// </summary>
        public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
        {
            if (service.IsGenericType)
            {
                var genericType = service.GetGenericTypeDefinition();
                var genericArguments = genericType.GetGenericArguments();
                if (genericArguments.Count() == 1
                 && genericArguments.Single().GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant))
                {
                    var argument = service.GetGenericArguments().Single();
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                           && kvp.Key.GetGenericTypeDefinition().Equals(genericType)
                                                           && kvp.Key.GetGenericArguments().Single() != argument
                                                           && kvp.Key.GetGenericArguments().Single().IsAssignableFrom(argument))
                        .SelectMany(kvp => kvp.Value);
                    return matches;
                }
            }

            return Enumerable.Empty<IBinding>();
        }
    }
}
