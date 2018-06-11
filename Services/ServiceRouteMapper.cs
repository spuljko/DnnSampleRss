
using Demo.Modules.CustomFeeds.Helpers;
using DotNetNuke.Web.Api;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Demo.Modules.CustomFeeds.Services
{

    /// <summary>
    /// The ServiceRouteMapper tells the DNN Web API Framework what routes this module uses
    /// </summary>
    public class ServiceRouteMapper : IServiceRouteMapper
    {
        /// <summary>
        /// RegisterRoutes is used to register the module's routes
        /// </summary>
        /// <param name="mapRouteManager"></param>
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            //mapRouteManager.MapHttpRoute(
            //    moduleFolderName: "CustomFeeds",
            //    routeName: "default",
            //    url: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional },
            //    namespaces: new[] { "Demo.Modules.CustomFeeds.Services" });


            mapRouteManager.MapHttpRoute(
                moduleFolderName: "CustomFeeds",
                routeName: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                namespaces: new string[] { "Demo.Modules.CustomFeeds.Services" });


            GlobalConfiguration.Configuration.Formatters.Add(new SyndicationFeedFormatter());
        }
    }
    
}