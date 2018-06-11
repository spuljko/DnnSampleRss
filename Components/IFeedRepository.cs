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

using System.Linq;
using DotNetNuke.Collections;

namespace Demo.Modules.CustomFeeds.Components
{
    public interface IFeedRepository
    {

        int Add(Feed t);

        void Delete(int id);

        void Delete(Feed t);

        Feed GetById(int id);

        IQueryable<Feed> GetAll();

        IPagedList<Feed> Query(string searchTerm, int pageIndex, int pageSize);

        void Update(Feed t);
    }
}