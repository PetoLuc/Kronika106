using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Kronika106.Models;

namespace Kronika106.Account
{
    public partial class ResetPassword : Page
    {
        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                //prihlasaný nema preco obnovovat heslo
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);            
            if (code != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = manager.FindByEmail(Email.Text);
                if (user == null)
                {
                    ErrorMessage.Visible = true;
                    FailureTextContent.Text = "Užívateľ neexistuje";
                    return;
                }
                var result = manager.ResetPassword(user.Id, code, PasswordToReset.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/ResetPasswordConfirmation");
                    return;
                }

                ErrorMessage.Visible = true;
                FailureTextContent.Text = result.Errors.FirstOrDefault();
                return;
            }
            else
            {
                //nikto pastol link do browsera takze ho presmetujeme prec
                Response.Redirect("~/Default");
                
            }
            //ErrorMessage.Visible = true;
            
        }
    }
}