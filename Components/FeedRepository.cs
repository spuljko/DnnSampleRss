/*
' Copyright (c) 2018 Synergos
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Collections;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;

namespace Demo.Modules.CustomFeeds.Components
{
    public class FeedRepository : ServiceLocator<IFeedRepository, FeedRepository>, IFeedRepository
    {

        protected override Func<IFeedRepository> GetFactory()
        {
            return () => new FeedRepository();
        }

        public int Add(Feed t)
        {
            Requires.NotNull(t);

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Feed>();
                rep.Insert(t);
            }
            return t.FeedId;
        }

        public void Delete(Feed t)
        {
            Requires.NotNull(t);
            Requires.PropertyNotNegative(t, "FeedId");

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Feed>();
                rep.Delete(t);
            }
        }

        public void Delete(int id)
        {
            Requires.NotNegative("feedId", id);

            var t = GetById(id);
            Delete(t);
        }

        public Feed GetById(int id)
        {
            Requires.NotNegative("id", id);
            
            Feed t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Feed>();
                t = rep.GetById(id);
            }
            return t;
        }

        public IQueryable<Feed> GetAll()
        {
            IQueryable<Feed> t = null;

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Feed>();

                t = rep.Get().AsQueryable();
            }

            return t;
        }

        public IPagedList<Feed> Query(string searchTerm, int pageIndex, int pageSize)
        {
            
            var t = GetAll().Where(c => c.Title.Contains(searchTerm)
                                                || c.Description.Contains(searchTerm));


            return new PagedList<Feed>(t, pageIndex, pageSize);
        }

        public void Update(Feed t)
        {
            Requires.NotNull(t);
            Requires.PropertyNotNegative(t, "FeedId");

            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Feed>();
                rep.Update(t);
            }
        }
       
    }
}