using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kronika106
{
    public partial class Search : System.Web.UI.UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Session["lastSearch"]!=null)
                txtSearch.Attributes.Add("placeholder", (string)Session["lastSearch"]);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            txtSearch.Text = "";
            if (string.IsNullOrWhiteSpace(searchText))
            { return; }
            Session["lastSearch"] = searchText;
            txtSearch.Attributes.Add("placeholder", searchText);

            if (Properties.Settings.Default.EnableStatistic)
            {
                string userId = Context.User.Identity.Name;
                using (var _db = new Kronika106DBContext())
                {
                    ApplicationUser user = _db.Users.First(u => u.UserName == userId);
                    if (user != null)
                    {
                        _db.StatisticsSearch.Add(new StatisticsSearch() { ApplicationUser = user, CreatedUTC = DateTime.UtcNow, SearchPattern = searchText });
                        _db.SaveChanges();
                    }
                }
            }


            //  lgSearch.SearchAll(searchText);
            Response.Redirect(Server.UrlPathEncode(string.Format("~/SearchResult.aspx?search={0}", searchText)));
        }
    }
}