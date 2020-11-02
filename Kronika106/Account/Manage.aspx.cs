using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using Kronika106.Models;

namespace Kronika106.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        //public int LoginsCount { get; set; }

        protected void Page_Load()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect(GlobalConstants.urlDefault);
            }
            SuccesMessage.Visible = false;
            if (!IsPostBack)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string userId = Context.User.Identity.Name;

                ApplicationUser aUser = null;

                using (var _db = new Kronika106.Models.Kronika106DBContext())
                {
                    aUser = _db.Users.FirstOrDefault(u => u.UserName == userId);
                }
                if (aUser != null)
                {
                    email.Text = aUser.Email;
                    firstame.Text = aUser.FirstName;
                    lastname.Text = aUser.LastName;
                    nick.Text = aUser.ScoutNickName;
					chSendMail.Checked = aUser.SendMail;
                    if (!string.IsNullOrEmpty(aUser.PasswordHash))
                    {
                        ChangePassword.Visible = true;
                    }
                    else
                    {                        
                        ChangePassword.Visible = false;
                    }
                    // Render success message
                    var message = Request.QueryString["m"];
                    if (message != null)
                    {
                        // Strip the query string from action
                        Form.Action = ResolveUrl("~/Account/Manage");

                        SuccessMessage =
                            message == "ChangePwdSuccess" ? "Tvoje heslo bolo zmenené."
                            : message == "SetPwdSuccess" ? "Tvoje heslo bolo nastavené."
                            : message == "RemoveLoginSuccess" ? "Účet bol odstránený."
                            : String.Empty;
                        SuccesMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                    }
                }
                else
                {
                    Response.Redirect(GlobalConstants.urlDefault);
                }
            }
        }

		protected void btnSave_Click(object sender, EventArgs e)
		{
			using (var _db = new Kronika106.Models.Kronika106DBContext())
			{
				string userId = Context.User.Identity.Name;
				ApplicationUser aUser = _db.Users.FirstOrDefault(u => u.UserName == userId);
				aUser.SendMail = chSendMail.Checked;
				_db.SaveChanges();
			}
		}	
	}
}