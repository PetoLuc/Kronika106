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
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //kontrola ci je user prihlaseny
                Session[GlobalConstants.RedirectURLKey] = HttpContext.Current.Request.Url.PathAndQuery;
                if (!Context.User.Identity.IsAuthenticated)
                {
                    Response.Redirect(GlobalConstants.urlForbidden, true);
                    return;
                }
            }
            catch (ThreadAbortException)
            {
            }
        }
        public List<UserRecord> LoadUsers()
        {
            IEnumerable<UserRecord> query = null;
            using (var _db = new Kronika106.Models.Kronika106DBContext())
            {
                query = from ApplicationUser usr in _db.Users where usr.EmailConfirmed==true orderby usr.ScoutNickName ascending, usr.FirstName ascending select new UserRecord {UserId = usr.Id, FirstName = usr.FirstName, LastName = usr.LastName, ScoutNickName = usr.ScoutNickName, Comments = usr.Comments.Count };

                if (query != null && query.Count() > 0)
                {
                    return query.ToList();
                }
            }
            return null;
        }
      
    }
}