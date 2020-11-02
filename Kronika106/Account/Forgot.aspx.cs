using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Kronika106.Models;
using System.Text;

namespace Kronika106.Account
{
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // var ss = IsValid;
        }

        protected void Forgot(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user's email address
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = manager.FindByEmail(Email.Text);
                if (user == null)
                {
                    FailtureTextContent.Text = "Uzívateľ neexistuje.";
                    ErrorMessage.Visible = true;
                    return;
                }

                if (!manager.IsEmailConfirmed(user.Id))
                {
                    FailtureTextContent.Text = "Uzívateľ nemá potvrdenú registráciu.";
                    ErrorMessage.Visible = true;
                    return;
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send email with the code and the redirect to reset password page
                string code = manager.GeneratePasswordResetToken(user.Id);                
                string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                //string test = Server.UrlDecode(callbackUrl);
                StringBuilder mailBodySB = new StringBuilder();
                mailBodySB.AppendLine(string.Format("Pre obnovu hesla pokračuj kliknutím <a href=\"{0}\"><b>SEM</b></a><br>", callbackUrl));                
                mailBodySB.AppendLine("Kronika 106 zboru Akataleptik Detva");
                manager.SendEmail(user.Id, "Obnova hesla pre kroniku 106",mailBodySB.ToString());
                loginForm.Visible = false;
                ErrorMessage.Visible = false;
                DisplayEmail.Visible = true;
            }
        }
    }
}