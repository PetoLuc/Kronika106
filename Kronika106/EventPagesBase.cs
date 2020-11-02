using Kronika106.Logic;
using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace Kronika106
{
    public abstract class EventPagesBase : Page
    {
        protected string FileSystemPath { get; set; }
        protected string RelativePath;
        protected string[] EventIdParams = null;
        protected string PageTitleBase = null;
        protected string StrPageHeader = null;

        protected abstract int NumberOfParams { get; }


        //    public void ScrollTo(string controllCLientId)
        //    {

        //        this.RegisterClientScriptBlock("ScrollTo", string.Format(@"
        //	<script type='text/javascript'> 

        //		$(document).ready(function() {{
        //			var element = document.getElementById('{0}');
        //			element.scrollIntoView();
        //			element.focus();
        //		}});

        //	</script>

        //", controllCLientId));
        //    }


        ////TimerRefreshForum.Interval = Properties.Settings.Default.ForumAutoRefrestInterval;
        ////TimerRefreshForum.Enabled = true;


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string pageName = string.Empty;
                try
                {                    
                    //zruisenie cache
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                    Response.Cache.SetNoStore();

                    //kontrola ci bol poslany request param
                    Session[GlobalConstants.RedirectURLKey] = HttpContext.Current.Request.Url.PathAndQuery;                    
                    if (Request.QueryString.Count == 0 || string.IsNullOrEmpty(QueryStringHelper.GetIdFromRequest(Request)))
                    {
                        Response.Redirect(GlobalConstants.urlDefault, true);
                        return;
                    }

                    //kontrola ci je user prihlaseny
                    if (!Context.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect(GlobalConstants.urlForbidden, true);
                        return;
                    }

                    //nacitanie filesystem query
                    string trueQuery = QueryStringHelper.GetIdFromRequest(Request);

                    //kontrola ci existuje filesystem struktura podla query
                    RelativePath = string.Format("{0}/{1}", GlobalConstants.PthFileSystemRoot, trueQuery);
                    FileSystemPath = Server.MapPath(RelativePath);
                    if (!System.IO.Directory.Exists(FileSystemPath))
                    {
                        Response.Redirect(GlobalConstants.urlDefault, true);
                        return;
                    }
                    EventIdParams = trueQuery.Split(GlobalConstants.EventIdSeparator, StringSplitOptions.RemoveEmptyEntries);
                    //pre akcia, akcia popis                        
                    if (EventIdParams != null && EventIdParams.Length == NumberOfParams )
                    {
                        StrPageHeader = EventIdParams[1];
                        PageTitleBase = string.Format("{0} - {1}", EventIdParams[0], EventIdParams[1]);
                    }              
                    else
                    {
                        Response.Redirect(GlobalConstants.urlDefault, true);
                        return;
                    }
                    Navigator.GenerateNavigation(Page.Master);

                    if (Properties.Settings.Default.EnableStatistic)
                    {
                        string userId = Context.User.Identity.Name;
                        using (var _db = new Kronika106DBContext())
                        {
                            ApplicationUser user = _db.Users.First(u => u.UserName == userId);
                            if (user != null)
                            {
                                _db.StatisticBrowse.Add(new StatisticBrowse() { ApplicationUser = user, CreatedUTC = DateTime.UtcNow, Url = Server.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery )});
                               _db.SaveChanges();
                            }
                        }
                    }

                }
                catch (ThreadAbortException)
                {
                }              
            }
        }

    }
}