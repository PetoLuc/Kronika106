using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Kronika106.Models;
using System.Text;
using Kronika106.Logic;

namespace Kronika106.Account
{
    public partial class Register : Page
    {
        CommonLogic commonLogic = new CommonLogic();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                //prihlasaný sa memoze znovu prihlasit
                Response.Redirect("~/Default.aspx");
            }     
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            string userNameNick = string.Format("{0} {1}", firstame.Text, lastname.Text);

            string error = string.Empty;
            if (commonLogic.IsDuplicateNick(nick.Text, email.Text, out error))
            {
                ErrorMessage.Visible = true;
                FailtureTextContent.Text = error;
                return;
            }
			
			var user = new ApplicationUser() {Id= Guid.NewGuid().ToString("N"), UserName = Guid.NewGuid().ToString("N"), Email = email.Text, NickName = userNameNick, ScoutNickName = nick.Text, FirstName = firstame.Text, LastName = lastname.Text, SendMail = chSendMail.Checked};
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                string code = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                commonLogic.SendRegisterConfirmationEmail(manager, user, callbackUrl);
                

                if (user.EmailConfirmed)
                {
                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    System.Web.Security.FormsAuthentication.SetAuthCookie(user.UserName, false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Visible = false;
                    InfoMessage.Visible = true;
                    InfoTextContent.Text = string.Format(GlobalConstants.EmailRegistrationSendOK, user.Email);
                }
            }
            else 
            {
                StringBuilder sbErrors = new StringBuilder();
                if (result.Errors != null)
                {
                    foreach (string err in result.Errors)
                    {
                        sbErrors.AppendLine(err);
                    }
                }
				ErrorMessage.Visible = true;
                FailtureTextContent.Text = sbErrors.ToString();
            }
        }

        
    }
}