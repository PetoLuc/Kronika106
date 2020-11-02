using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Kronika106.Models;

namespace Kronika106.Account
{
    public partial class ManagePassword : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            using (var _db = new Kronika106DBContext())
            {
                string userId = Context.User.Identity.Name;
                ApplicationUser user = _db.Users.First(u => u.UserName == userId);
                return manager.HasPassword(user.Id);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword(manager))
                {
                    changePasswordHolder.Visible = true;
                }
                else
                {
                    setPassword.Visible = true;
                    changePasswordHolder.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");
                }
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                using (var _db = new Kronika106DBContext())
                {
                    string userId = Context.User.Identity.Name;
                    ApplicationUser user = _db.Users.First(u => u.UserName == userId);

                    IdentityResult result = manager.ChangePassword(user.Id, CurrentPassword.Text, NewPassword.Text);
                    if (result.Succeeded)
                    {
                        //var user = manager.FindById(User.Identity.GetUserId());
                        signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
        }

        protected void SetPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Create the local login info and link the local account to the user
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var _db = new Kronika106DBContext();
                string userId = Context.User.Identity.Name;
                ApplicationUser user = _db.Users.First(u => u.UserName == userId);


                IdentityResult result = manager.AddPassword(user.Id, password.Text);
                if (result.Succeeded)
                {                    
                    user.EmailConfirmed = true;
                    _db.SaveChanges();
                    Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
                }
                else
                {
                    AddErrors(result);
                }
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}