using Kronika106.Logic;
using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kronika106
{
    //http://www.codeproject.com/Articles/9731/How-to-scroll-an-ASP-NET-control-into-view-after-p

    public partial class ForumControll : System.Web.UI.UserControl
    {
        public List<PageContent> ContentList = new List<PageContent>();
        const string hrmlColmd12 = "<div class=\"col-md-12\">";
        const string hrmlColmd11_colmdoffset1 = "<div class=\"col-md-11 col-xs-11 col-lg-11 col-sm-11 col-md-offset-1 col-xs-offset-1 col-lg-offset-1 col-sm-offset-1\">";
        const string hrmlColmd10_colmdoffset2 = "<div class=\"col-md-10 col-md-offset-2 col-xs-offset-2 col-lg-offset-2 col-sm-offset-2\">";
        const string htmlRow = "<div class=\"row\" id=rootRow{0}>";
        const string htmlFormGroup = "<div class=\"form-group\">";
        const string htmlEndP = "</p>";
        const string htmlEndDiv = "</div>";
        const string image = "<a id=\"bigImage\" href=\"javascript: void(0);\" onclick=\"javascript: showModalImageClick(this);\"> <img src = \"{0}\" class=\"img_left\" style=\"max-height:120px; max-width:120px; width: auto; height: auto\"></a>";
        const string htmlComment = "<p id=\"comment_{0}\" style=\"line-height: 1.1; text-align:justify; word-wrap:break-word;\">{1} {2} {4} {5}<br>{3}";
        const string htmlReplyLink = "<a class=\"ReplyLink\" title=\"Odpovedaj\" href=\"javascript: void(0);\" id=\"btnReply_{0}\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"javascript: showModalClick(this);\"><i class=\"fa fa-reply\"></i></a>";
        const string htmlEditLink = "<a class=\"EditLink\" title=\"Uprav komentár\"href=\"javascript: void(0);\" id=\"btnEdit_{0}\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"javascript: showModalClick(this);\"><i class=\"fa fa-pencil-square-o\"></i></a>";
        const string htmlDeleteLink = "<a class=\"DeleteLink\" title=\"Zmaž komentár\"href=\"javascript: void(0);\" id=\"btnDelete_{0}\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"javascript: showModalClick(this);\"><i class=\"fa fa-trash\"></i></a>";
        const string htmlHr = "<hr>";

        public enum CommentLevel
        {
            Root = 1,
            Reply = 2,
            ReplyToReply = 3,
        }

        public enum ForumMode
        {
            Reload = 0,
            Append = 1
        }
        public enum ForumType
        {
            Event = 1,
            EventPhotoGallery = 2,
            EventVideoGallery = 3,
            CommentList = 4
        }
        public class PageContent
        {
            public int ID { get; set; }
            public DateTime CreatedUTC { get; set; }
            public string EventId { get; set; }
            public string Comment { get; set; }
            public string ThumbPath { get; set; }
            public bool IsEvent { get; set; }
            public bool IsPhoto { get; set; }
            public bool IsVideo { get; set; }
            public bool IsFileSystem { get; set; }
            public int? RootID { get; set; }
            public string UserName { get; set; }
            public string UserID { get; set; }
            public string ScoutNickName { get; set; }
            public string NickName { get; set; }
            public int SearchRating { get; set; }

            //public TimeSpan? VideoPosition { get; set; }
        }

        public ForumType forumType
        {
            get; set;
        }

        public ForumMode forumMode
        {
            get; set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                //nacitanie comment ID ak je
                string commentId = QueryStringHelper.GetCommentIdFromRequest(Request);
                if (!string.IsNullOrEmpty(commentId))
                {
                    hiddenCommentID.Value = commentId;
                }
                if (forumType == ForumType.CommentList)
                {
                    addCommentRow.Visible = false;
                }
                GetComments(ContentList);
            }
        }                


        private string generateCommentTreeHTML(List<PageContent> lstCommnet, CommentLevel commentLevel)
        {
            StringBuilder resBuilder = new StringBuilder();
            foreach (var comment in lstCommnet)
            {

                resBuilder.AppendFormat(htmlRow, comment.ID);
                resBuilder.Append(htmlFormGroup);

                switch (commentLevel)
                {
                    case CommentLevel.Root:
                        resBuilder.Append(hrmlColmd12);
                        break;
                    case CommentLevel.Reply:
                        resBuilder.Append(hrmlColmd11_colmdoffset1);
                        break;
                    case CommentLevel.ReplyToReply:
                        resBuilder.Append(hrmlColmd10_colmdoffset2);
                        break;
                    default:
                        {
                            resBuilder.Append(commentLevel);
                        }
                        break;
                }

                string eventId = null;
                string url = null;
                //linky sa generuju len ked pride na vstup uz list komentarov
                if (!string.IsNullOrEmpty(comment.EventId) && forumType== ForumType.CommentList)
                {

                    string[] eventIdParams = comment.EventId.Split(GlobalConstants.EventIdSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (comment.IsEvent && eventIdParams.Length == 2)
                    {
                        eventId = $"{eventIdParams[0]} {eventIdParams[1]}";
                        url = $"AkciaPopis.aspx?{QueryStringHelper.KeyID}={HttpUtility.UrlEncode(comment.EventId)}&{QueryStringHelper.KeyCommentId}=comment_{comment.ID}";
                    }
                    if (comment.IsPhoto && eventIdParams.Length == 2)
                    {
                        eventId = $"{eventIdParams[0]} {eventIdParams[1]} galéria";
                        url = $"Akcia.aspx?{QueryStringHelper.KeyID}={HttpUtility.UrlEncode(comment.EventId)}&{QueryStringHelper.KeyCommentId}=comment_{comment.ID}";
                    }
                    if (comment.IsVideo && eventIdParams.Length == 4)
                    {
                        eventId = $"{eventIdParams[0]} {eventIdParams[1]} {eventIdParams[3]}";
                        url = $"Video.aspx?{QueryStringHelper.KeyID}={HttpUtility.UrlEncode(comment.EventId)}&{QueryStringHelper.KeyCommentId}=comment_{comment.ID}";
                    }
                    if (comment.IsFileSystem)
                    {
                        switch (eventIdParams.Length)
                        {
                            case 1: //rok
                                eventId = $"Rok {eventIdParams[0]}";
                                url = $"RokAkcie.aspx?{QueryStringHelper.KeyID}={HttpUtility.UrlEncode(comment.EventId)}";
                                break;
                            case 2: //akcia
                                eventId = $"Rok {eventIdParams[0]}, akcia {eventIdParams[1]}";
                                url = $"AkciaPopis.aspx?{QueryStringHelper.KeyID}={HttpUtility.UrlEncode(comment.EventId)}";
                                break;
                            case 4://video       
                                eventId = $"Rok {eventIdParams[0]}, akcia {eventIdParams[1]}, video {eventIdParams[3]}";
                                url = $"Video.aspx?{QueryStringHelper.KeyID}={HttpUtility.UrlEncode(comment.EventId)}";
                                break;
                           default:
                                break;
                        }
                    }
                }

                string user = string.Format("<strong>{0}</strong> {1} {2}",
                    (!string.IsNullOrEmpty(comment.ScoutNickName) ? comment.ScoutNickName : comment.NickName),
                   !comment.IsFileSystem ? CommonLogic.ToSKDateTime(comment.CreatedUTC) : string.Empty,
                    string.Format("<a href=\"{0}\">{1}</a>", url, eventId));
                string picture = null;

                if (!string.IsNullOrEmpty(comment.ThumbPath))
                {
                    picture = string.Format(image, comment.ThumbPath);
                }

                string replyLink = null;
                if (commentLevel != CommentLevel.ReplyToReply && forumType != ForumType.CommentList)
                {
                    replyLink = string.Format(htmlReplyLink, comment.ID);
                }

                string editLink = null;
                string deleteLink = null;
                if (comment.UserName == Context.User.Identity.Name && forumType != ForumType.CommentList)
                {
                    editLink = string.Format(htmlEditLink, comment.ID);
                    deleteLink = string.Format(htmlDeleteLink, comment.ID);
                }             

                resBuilder.AppendFormat(htmlComment, comment.ID, user, replyLink, picture, editLink, deleteLink);
                resBuilder.AppendLine(comment.Comment);
                resBuilder.AppendLine(htmlEndP);
                resBuilder.AppendLine(htmlEndDiv);
                resBuilder.AppendLine(htmlEndDiv);
                resBuilder.AppendLine(htmlEndDiv);

                if (forumType != ForumType.CommentList)
                {

                    List<PageContent> lstChild = null;
                    using (var _db = new Kronika106.Models.Kronika106DBContext())
                    {
                        lstChild = (from EventComments childComment in _db.Forum
                                    where childComment.RootID == comment.ID
                                    orderby comment.ID descending
                                    select new PageContent { ID = childComment.ID, CreatedUTC = childComment.CreatedUTC, EventId = childComment.EventId, RootID = childComment.RootID, ThumbPath = childComment.ThumbPath, ScoutNickName = childComment.ApplicationUser.ScoutNickName, NickName = childComment.ApplicationUser.NickName, Comment = childComment.Comment, IsVideo = childComment.IsVideo, IsEvent = comment.IsEvent, IsPhoto = comment.IsPhoto, UserID = childComment.ApplicationUser.Id, UserName = childComment.ApplicationUser.UserName/*, VideoPosition= comment.VideoPosition */}).ToList();
                    }
                    if (lstChild.Count > 0)
                    {
                        resBuilder.AppendLine(generateCommentTreeHTML(lstChild, commentLevel + 1));
                    }
                }
                //else
                //{
                //    resBuilder.AppendLine(htmlHr);
                //}              
            }
            return resBuilder.ToString();
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Vráti všetky fotkove komentáre pre danú akciu
        /// </summary>        
        /// <returns> vrati html sranku s komentarmi</returns>
        public void GetComments(List<PageContent> contentList = null)
        {
            string trueEvetId = null; ;
            if (forumType != ForumType.CommentList)
            {
                if (string.IsNullOrEmpty(Request.QueryString["ID"]))
                {

                }
                trueEvetId = Server.UrlDecode(Request.QueryString["ID"]);

                if (string.IsNullOrEmpty(trueEvetId))
                {
                    return;
                }
                trueEvetId = trueEvetId.ToLower();
            }


            //len na kratko do DB            
            if (forumType != ForumType.CommentList)
            {
                using (var _db = new Kronika106.Models.Kronika106DBContext())
                {
                    contentList = (from EventComments comment in _db.Forum
                                   where
                                     comment.RootID == null && ((comment.EventId.ToLower() == trueEvetId && ((forumType == ForumType.CommentList) || (forumType == ForumType.EventPhotoGallery) == comment.IsPhoto) && (((forumType == ForumType.Event) == comment.IsEvent) && ((forumType == ForumType.EventVideoGallery) == comment.IsVideo))))
                                   orderby comment.ID descending
                                   select new PageContent { ID = comment.ID, CreatedUTC = comment.CreatedUTC, EventId = comment.EventId, RootID = comment.RootID, ThumbPath = comment.ThumbPath, ScoutNickName = comment.ApplicationUser.ScoutNickName, NickName = comment.ApplicationUser.NickName, Comment = comment.Comment, IsVideo = comment.IsVideo, IsEvent = comment.IsEvent, IsPhoto = comment.IsPhoto, UserID = comment.ApplicationUser.Id, UserName = comment.ApplicationUser.UserName/*, VideoPosition= comment.VideoPosition */}).ToList();
                }
            }
            if (contentList.Count > 0)
            {
                // resultHtml.Append("<div class=\"inner-content\">");
                if (forumMode == ForumMode.Append)
                {
                    phComments.InnerHtml += generateCommentTreeHTML(contentList, CommentLevel.Root);
                }
                else
                {
                    phComments.InnerHtml = generateCommentTreeHTML(contentList, CommentLevel.Root);
                }
            }
            commentsContainer.Visible = true;
            if (string.IsNullOrWhiteSpace(phComments.InnerHtml))
            {
                commentsContainer.Visible = false;
            }

        }


        //prida komentár do databázy
        public async void AddComment(string commentContent, string thumbPath, string idRootEvetStr)
        {
            if (!Context.User.Identity.IsAuthenticated || string.IsNullOrEmpty(commentContent))
            {
                return;
            }            

            #region videoTime
            //TimeSpan? videoTime=null;
            //if (hiddenVideoPosition.Value != null)
            //{
            //    string seconds = hiddenVideoPosition.Value.Split('.')[0];
            //    if (!string.IsNullOrEmpty(seconds))
            //    {
            //        int sec = -1;
            //        if (int.TryParse(seconds, out sec))
            //        {
            //            videoTime = new TimeSpan(0, 0, sec);
            //        }
            //    }
            //}
            #endregion videoTime

            string trueQueryString = Server.UrlDecode(Request.QueryString["ID"]);            
            using (var _db = new Kronika106DBContext())
            {
                int idRootComment = -1;
                EventComments rootComment = null;
                if (!string.IsNullOrEmpty(idRootEvetStr))
                {
                    int.TryParse(idRootEvetStr, out idRootComment);
                    if (idRootComment > 0)
                    {
                        rootComment = _db.Forum.FirstOrDefault(c => c.ID == idRootComment);
                        trueQueryString = rootComment.EventId;
                    }
                }
                if (string.IsNullOrWhiteSpace(trueQueryString))
                {
                    return;
                }
                string userId = Context.User.Identity.Name;
                ApplicationUser user = _db.Users.First(u => u.UserName == userId);
                EventComments newComment = new EventComments() { CreatedUTC = DateTime.UtcNow, EventId = trueQueryString, Comment = commentContent, ApplicationUser = user, ThumbPath = thumbPath, IsPhoto = forumType == ForumType.EventPhotoGallery, IsEvent = forumType == ForumType.Event, IsVideo = forumType == ForumType.EventVideoGallery, RootID = rootComment?.ID/*, VideoPosition= videoTime*/};
                _db.Forum.Add(newComment);
                await _db.SaveChangesAsync();             
            }
            GetComments();
        }


        /// <summary>
        /// aktualizuje komentar v databaze
        /// </summary>
        /// <param name="commentId">id aktualizovaneho komentara</param>
        /// <param name="updatedContent">novy obsah</param>
        public async void UpdateComment(string commentId, string updatedContent)
        {
            if (!Context.User.Identity.IsAuthenticated || string.IsNullOrEmpty(updatedContent) || string.IsNullOrEmpty(commentId))
            {
                return;
            }
            using (var _db = new Kronika106DBContext())
            {
                EventComments curComment = null;
                int commentIdInt = -1;
                int.TryParse(commentId, out commentIdInt);
                if (commentIdInt <= 0)
                {
                    return;
                }
                curComment = _db.Forum.FirstOrDefault(c => c.ID == commentIdInt);
                if (curComment == null)
                {
                    return;
                }

                //ak ten co meni komentar nie je jeho majitel
                if (curComment.ApplicationUser.UserName != Context.User.Identity.Name)
                {
                    return;
                }
                curComment.Comment = updatedContent;
                await _db.SaveChangesAsync();                                                                                          
            }
            GetComments();
        }

        public async void DeleteComment(string commentId)
        {
            if (!Context.User.Identity.IsAuthenticated || string.IsNullOrEmpty(commentId))
            {
                return;
            }
            using (var _db = new Kronika106DBContext())
            {
                EventComments curComment = null;
                int commentIdInt = -1;
                int.TryParse(commentId, out commentIdInt);
                if (commentIdInt <= 0)
                {
                    return;
                }
                curComment = _db.Forum.FirstOrDefault(c => c.ID == commentIdInt);
                if (curComment == null)
                {
                    return;
                }
                //ak ten co meni komentar nie je jeho majitel
                if (curComment.ApplicationUser.UserName != Context.User.Identity.Name)
                {
                    return;
                }
                var lstChilds = _db.Forum.Where(c => c.RootID == commentIdInt);
                foreach (var child in lstChilds)
                {
                    child.RootID = curComment.RootID;
                    child.ThumbPath = curComment.ThumbPath;
                    child.ApplicationUser = child.ApplicationUser;
                }
                _db.Forum.Remove(curComment);
                await _db.SaveChangesAsync();                
            }
            GetComments();
        }


        protected void btnSendReply_Click(object sender, EventArgs e)
        {            
            string comment = reply.Text;
            reply.Text = string.Empty;                                       

            AddComment(comment, null, hiddenCommentID.Value);
            hiddenCommentID.Value = null;
            btnCommitComment.Enabled = true;
            //btnCommitComment.Enabled = true;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {                                 
            string comment = txtNewComment.Text;
            txtNewComment.Text = string.Empty;
            AddComment(comment,hiddenSliderId.Value, null);            
            btnSend.Enabled = true;
        }

        protected void TimerRefreshForum_Tick(object sender, EventArgs e)
        {
           GetComments();
        }

        protected void btnCommintUpdate_Click(object sender, EventArgs e)
        {
            string updatetContent = reply.Text;
            reply.Text = string.Empty;            
            UpdateComment(hiddenCommentID.Value, updatetContent);
            btnCommintUpdate.Enabled = true;   
            
        }

        protected void btnComitDelete_Click(object sender, EventArgs e)
        {
            DeleteComment(hiddenCommentID.Value);
            btnComitDelete.Enabled = true;
        }
    }
}