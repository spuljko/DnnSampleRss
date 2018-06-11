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
using Demo.Modules.CustomFeeds.Components;
using DotNetNuke.Entities.Users;
using Newtonsoft.Json;
using System.Linq;

namespace Demo.Modules.CustomFeeds.Services.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FeedViewModel
    {
        public FeedViewModel(Feed t, int portalID)
        {
            FeedId = t.FeedId;
            Title = t.Title;
            Description = t.Description;
            Address = t.Address;

            var userlist = DotNetNuke.Entities.Users.UserController.GetUsers(portalID);
            var usr = userlist.Cast<UserInfo>().Where(u => u.UserID == t.CreatedByUserId).FirstOrDefault();
            CreatedBy = userlist.Cast<UserInfo>().Where(u => u.UserID == t.CreatedByUserId).Select(u => u.DisplayName).FirstOrDefault();
            
            CreatedDate = t.CreatedOnDate;
        }
        
        public FeedViewModel() { }

        [JsonProperty("FeedId")]
        public int FeedId { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Description")]

        public string Description { get; set; }

        [JsonProperty("Address")]

        public string Address { get; set; }

        [JsonProperty("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}