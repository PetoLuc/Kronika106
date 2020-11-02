using Kronika106.Logic;
using Kronika106.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kronika106.Admin
{
    public class UserRecord
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ScoutNickName { get; set; }
        public bool SendNewsMail { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Comments{ get; set; }
        public DateTime? LastLogin { get; set; }
        public int? LoginCount { get; set; }

        public string SearchPattrern { get; set; }
        public string BrowseURL { get; set; }
        public DateTime? LastAccess { get; set; }
    }


    public partial class AdminMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated || !HttpContext.Current.User.IsInRole(GlobalConstants.RoleAdmin))
            {
                Response.Redirect(GlobalConstants.urlDefault);
            }
            ErrorMessage.Visible = false;
            InfoMessage.Visible = false;
        }

        public List<UserRecord> LoadUsers()
        {
            IEnumerable<UserRecord> query = null;
            using (var _db = new Kronika106.Models.Kronika106DBContext())
            {
                query = from ApplicationUser usr in _db.Users select new UserRecord { FirstName = usr.FirstName, LastName = usr.LastName, ScoutNickName = usr.ScoutNickName, SendNewsMail = usr.SendMail, Email = usr.Email, EmailConfirmed = usr.EmailConfirmed, Comments = usr.Comments.Count, LastLogin = usr.LastLogInUTC, LoginCount= usr.LoginCount };

                if (query != null && query.Count() > 0)
                {
                    return query.ToList();
                }
            }
            return null;
        }

        public List<UserRecord> LoadSearch()
        {
            IEnumerable<UserRecord> query = null;
            using (var _db = new Kronika106.Models.Kronika106DBContext())
            {
                query = from StatisticsSearch sch in _db.StatisticsSearch select new UserRecord { FirstName = sch.ApplicationUser.FirstName, LastName = sch.ApplicationUser.LastName, ScoutNickName = sch.ApplicationUser.ScoutNickName, SearchPattrern = sch.SearchPattern};

                if (query != null && query.Count() > 0)
                {
                    return query.ToList();
                }
            }
            return null;
        }

        public List<UserRecord> LoadBrowse()
        {
            IEnumerable<UserRecord> query = null;
            using (var _db = new Kronika106.Models.Kronika106DBContext())
            {
                query = from StatisticBrowse sch in _db.StatisticBrowse orderby sch.CreatedUTC descending select new UserRecord { FirstName = sch.ApplicationUser.FirstName, LastName = sch.ApplicationUser.LastName, ScoutNickName = sch.ApplicationUser.ScoutNickName, BrowseURL = sch.Url, LastAccess = sch.CreatedUTC };

                if (query != null && query.Count() > 0)
                {
                    return query.ToList();
                }
            }
            return null;
        }
        public async void Unnamed_Click(object sender, EventArgs e)
        {
            try
            {
                progress.Text = "";
                ErrorMessage.Visible = false;
                InfoMessage.Visible = false;
                using (var _db = new Kronika106.Models.Kronika106DBContext())
                {
                    IEnumerable<string> query = from ApplicationUser usr in _db.Users where (usr.SendMail || chSendToAll.Checked) && usr.EmailConfirmed select usr.Email;
                    if (query == null|| query.Count()==0)
                    {
                        ErrorMessage.Visible = true;
                        FailtureTextContent.Text = "Neexistujú užívatelia, ktorí odoberajú info maily";
                        return;
                    }

                    EmailService emailService = new EmailService();

                    foreach (var val in query)
                    {
                        await emailService.SendAsync(new Microsoft.AspNet.Identity.IdentityMessage() { Subject = subject.Text, Body = body.Text, Destination = (string)val });
                        progress.Text = progress.Text + string.Format("Mail pre: {0} odoslaný{1}", (string)val, Environment.NewLine);
                    }
                    InfoMessage.Visible = true;
                    InfoTextContent.Text = "Maily boli odoslané";
                    subject.Text = "";
                    body.Text = "";


                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(ex, "AdminMaster");
                ErrorMessage.Visible = true;
                FailtureTextContent.Text = ex.Message;
            }
        }

        protected void refreshUser_Click(object sender, EventArgs e)
        {
            this.DataBind();
        }
    }
}