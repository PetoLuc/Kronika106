using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Kronika106.Models;
using Kronika106.Logic;
using System.Text;

namespace Kronika106.Account
{
    public partial class Confirm : Page
    {
        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            string userId = IdentityHelper.GetUserIdFromRequest(Request);
            if (code != null && userId != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var result = manager.ConfirmEmail(userId, code);
                if (result.Succeeded)
                {
                    SuccesMessage.Visible = true;
                    return;
                }
                else
                {
                    //TODO zalogovast dovod           
                    StringBuilder errorsSB = null;
                    if (result.Errors != null)
                    {
                        errorsSB = new StringBuilder();
                        foreach (string error in result.Errors)
                        {
                            errorsSB.AppendLine(error);
                        }                        
                    }
                    ExceptionUtility.LogException(new Exception($"manager.ConfirmEmail return error: {errorsSB?.ToString()}, for userId: {userId}, code: {code}"), "Confirm.aspx");

                    SuccesMessage.Visible = false;
                    ErrorMessage.Visible = true;
                    FailtureTextContent.Text = "Nastala chyba pri potvrdení emailu";
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
            
        }
    }
}