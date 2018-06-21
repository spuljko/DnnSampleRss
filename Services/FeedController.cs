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
using log4net;
using DotNetNuke.Instrumentation;

namespace Demo.Modules.CustomFeeds.Services
{
    //[SupportedModules("CustomFeeds")]
    //[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Anonymous)]
    public class FeedController : DnnApiController, IDisposable
    {
        private readonly IFeedRepository _repository;
        private IContractResolver _resolver;

        public FeedController(IFeedRepository repository)
        {
            Requires.NotNull(repository);

            this._repository = repository;
            
        }

        public FeedController() : this(FeedRepository.Instance) { }


        [DnnAuthorize()]
        [HttpGet()]
        public HttpResponseMessage HelloWorld()
        {
            try
            {
                LoggerSource.Instance.GetLogger(typeof(FeedController)).Fatal("test mesage");
                throw new Exception("Test excetion");
                string helloWorld = "Hello World!";
                return Request.CreateResponse(HttpStatusCode.OK, helloWorld);
            }
            catch (System.Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [DnnAuthorize()]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage Delete(int feedId)
        {
            try
            {
                
                var item = _repository.GetById(feedId);

                _repository.Delete(item);

                return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
            }
            catch (System.Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage Get(int feedId)
        {
            try
            {
                var item = new FeedViewModel(_repository.GetById(feedId), this.PortalSettings.PortalId);

                return Request.CreateResponse(item);
            }
            catch (System.Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [DnnAuthorize()]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage Delete(FeedViewModel feedViewModel)
        {
            try
            {
                var item = _repository.GetById(feedViewModel.FeedId);

                _repository.Delete(item);

                return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
            }
            catch (System.Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [DnnAuthorize()]
        [HttpGet]
        public HttpResponseMessage GetList()
        {
            List<FeedViewModel> items;

            try
            {
                LoggerSource.Instance.GetLogger(typeof(FeedController)).Trace("GetList called");
                items = _repository.GetAll()
                       .Select(item => new FeedViewModel(item, this.PortalSettings.PortalId))
                       .ToList();
                //var jsonSettings = new JsonSerializerSettings()
                //{
                //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                //};
                var ret = JsonConvert.SerializeObject(items, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, ret, "application/json");
            }
            catch (System.Exception ex)
            {
                //Log to DotNetNuke and reply with Error
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [DnnAuthorize()]
        [HttpGet]
        public HttpResponseMessage Read(int skip, int take)
        {
            var items = _repository.GetAll()
                       .Select(item => new FeedViewModel(item, this.PortalSettings.PortalId))
                       .ToList();

            var result = items.Skip(skip).Take(take);
            var jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var ret = JsonConvert.SerializeObject(result, Formatting.None, jsonSettings);
            return Request.CreateResponse(ret);
        }

        [HttpPost]
        [DnnAuthorize()]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage Upsert(FeedViewModel item)
        {
            try
            {
                if (item.FeedId > 0)
                {
                    var t = Update(item);
                    return Request.CreateResponse(System.Net.HttpStatusCode.NoContent);
                }
                else
                {
                    var t = Create(item);
                    return Request.CreateResponse(t.FeedId);
                }
            }
            catch (System.Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private Feed Create(FeedViewModel item)
        {
            Feed t = new Feed
            {
                Title = item.Title,
                Description = item.Description,
                Address = item.Address,
                //ModuleId = ActiveModule.ModuleID,
                CreatedByUserId = UserInfo.UserID,
                LastModifiedByUserId = UserInfo.UserID,
                CreatedOnDate = DateTime.UtcNow,
                LastModifiedOnDate = DateTime.UtcNow
            };
            _repository.Add(t);

            return t;
        }

        private Feed Update(FeedViewModel item)
        {
            var t = _repository.GetById(item.FeedId);
            if (t != null)
            {
                t.Title = item.Title;
                t.Description = item.Description;
                t.Address = item.Address;
                t.LastModifiedByUserId = UserInfo.UserID;
                t.LastModifiedOnDate = DateTime.UtcNow;
            }
            _repository.Update(t);

            return t;
        }
    }


}
