using Kronika106.Admin;
using Kronika106.Logic;
using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kronika106
{
    public partial class LastComments : System.Web.UI.Page
    {
        private const int pageSize = 100;
        private const int maxCommentCount = 5000;        

        public LastComments()
        {         
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                title.InnerText = "Najnovšie komentáre";
                //kontrola ci je user prihlaseny
                Session[GlobalConstants.RedirectURLKey] = HttpContext.Current.Request.Url.PathAndQuery;
                if (!Context.User.Identity.IsAuthenticated)
                {
                    Response.Redirect(GlobalConstants.urlForbidden, true);
                    return;
                }
                if (!IsPostBack)
                {
                    //prve nacitanie
                    if (Session[GlobalConstants.LoadedCommentsCount] == null)
                    {
                        Session[GlobalConstants.LoadedCommentsCount] = 0;
                    }

                    //if (Session[GlobalConstants.MaxId] == null)
                    {
                        Session[GlobalConstants.MaxId] = 0;
                    }

                    int loadFirstCount = (int)Session[GlobalConstants.LoadedCommentsCount];
                    Session[GlobalConstants.LoadedCommentsCount] = 0;

                    if (loadFirstCount > 0)
                    {
                        using (var _db = new Kronika106.Models.Kronika106DBContext())
                        {
                            int maxId = (int)Session[GlobalConstants.MaxId];
                            int newComments = _db.Forum.Count(c => c.ID > maxId);                            
                        }
                    }
                  ForumControll.ContentList.AddRange(LoadComments(0, loadFirstCount > pageSize ? loadFirstCount : pageSize));
                }
            }
            catch (ThreadAbortException)
            {
            }
        }
        public List<ForumControll.PageContent> LoadComments(int skip, int recordCount)
        {
            string userID = string.Empty;
            if (Request.QueryString.Count == 1)
            {
                userID = Request.QueryString["ID"];

                using (var _db = new Kronika106.Models.Kronika106DBContext())
                {
                    //ak nie je uvedene user id idem vzdy na komentare usera, alebo ak niekto zmanipuluje user id
                    var user = string.IsNullOrWhiteSpace(userID) ? null : _db.Users.FirstOrDefault(u => u.Id == userID);
                    if (user == null)
                    {
                        user = _db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                        //paranoid mode - toto by malo byt v DB vzdy
                        userID = user?.Id;
                    }
                    //paranoid mode - user id bude naplnene vzdy
                    if (!string.IsNullOrWhiteSpace(userID))
                    {
                        title.InnerText = "Komentáre " + (!string.IsNullOrEmpty(user.ScoutNickName) ? user.ScoutNickName : $"{user.FirstName} {user.LastName}");
                    }
                }
                //nastavenie user ID
            }

            List<ForumControll.PageContent> commentList = null;
            using (var _db = new Kronika106.Models.Kronika106DBContext())
            {
                int allCommentsCount = _db.Forum.Count();

                commentList = (from EventComments events in _db.Forum
                               where (userID != null && events.ApplicationUser.Id == userID) || (userID == string.Empty)
                               orderby events.CreatedUTC descending
                               select new ForumControll.PageContent { ID = events.ID, NickName = events.ApplicationUser.NickName, ScoutNickName = events.ApplicationUser.ScoutNickName, EventId = events.EventId, Comment = events.Comment, CreatedUTC = events.CreatedUTC, ThumbPath = events.ThumbPath, IsEvent = events.IsEvent, IsPhoto = events.IsPhoto, IsVideo = events.IsVideo, RootID = events.RootID }).Skip(skip).Take(recordCount > maxCommentCount ? maxCommentCount : recordCount).ToList();
            }
            if (commentList != null && commentList.Count() > 0)
            {
                Session[GlobalConstants.LoadedCommentsCount] = (int)Session[GlobalConstants.LoadedCommentsCount] + commentList.Count();

                StringBuilder resultHtml = new StringBuilder();
                
                int maxId = 0;
                

                maxId = commentList.Max(c => c.ID);
                
                Session[GlobalConstants.MaxId] = maxId;
                btnLoadNextComments.Enabled = true;

                
               // phComments.InnerHtml = phComments.InnerHtml + fc.GenerateCommentTreeHTML(commentList, ForumControll.CommentLevel.Root); 
            }
            else
            {
                btnLoadNextComments.Enabled = false;
            }
            return commentList;
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            int skip = (int)Session[GlobalConstants.LoadedCommentsCount];
           ForumControll.GetComments(LoadComments(skip, pageSize));
        }
    }
}