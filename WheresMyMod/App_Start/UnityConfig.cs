using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using WheresMyMod.Services;

namespace WheresMyMod
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            // RegisterType -- when they ask for this (IModsService) give them that(an instance of ModsService)
            container.RegisterType<IModsService, ModsService>();
            container.RegisterType<IGamesService, GamesService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}