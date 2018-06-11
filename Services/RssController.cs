using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Demo.Modules.CustomFeeds.Components;
using Demo.Modules.CustomFeeds.Services.ViewModels;
using DotNetNuke.Common;
using DotNetNuke.Web.Api;
using DotNetNuke.Security;
using System.Threading;
using DotNetNuke.UI.Modules;
using DotNetNuke.Common.Utilities;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net;
using DotNetNuke.Services.Exceptions;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Demo.Modules.CustomFeeds.Helpers;

namespace Demo.Modules.CustomFeeds.Services
{
    //[SupportedModules("CustomFeeds")]
    //[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
    [AllowAnonymous]
    public class RssController : DnnApiController, IDisposable
    {
        private readonly IFeedRepository _repository;

        public RssController(IFeedRepository repository)
        {
            Requires.NotNull(repository);

            this._repository = repository;

        }

        public RssController() : this(FeedRepository.Instance) { }


        //[DnnAuthorize(AllowAnonymous = true)]
        [HttpGet()]
        public HttpResponseMessage HelloWorld()
        {
            try
            {
                string helloWorld = "Hello World!";
                return Request.CreateResponse(HttpStatusCode.OK, helloWorld);
            }
            catch (System.Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet()]
        //[DnnAuthorize()]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
        //public IEnumerable<View> Get()
        public HttpResponseMessage Get()
        {
            IList<FeedViewModel> items;

            try
            {
                items = _repository.GetAll()
                       .Select(item => new FeedViewModel(item, this.PortalSettings.PortalId))
                       .ToList();
                
                return Request.CreateResponse(HttpStatusCode.OK, items, new SyndicationFeedFormatter(), "application/rss+xml");
            }
            catch (System.Exception ex)
            {
                //Log to DotNetNuke and reply with Error
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}