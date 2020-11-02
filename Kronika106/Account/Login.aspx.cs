using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Kronika106.Models;
using System.Web.Security;
using Kronika106.Logic;
using System.Linq;

namespace Kronika106.Account
{
    public partial class Login : Page
    {
        CommonLogic commonLogic = new CommonLogic();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                //prihlasaný sa memoze znovu prihlasit
                Response.Redirect(GlobalConstants.urlDefault);
            }

            RegisterHyperLink.NavigateUrl = "Register";
            ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //OpenAuthLogin.IsPersistend = RememberMe.Checked;
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["EventForbidden"]))
            {                
                FailureText.Text = "Pre prezeranie musíš byť prihlásený";
                ErrorMessage.Visible = true;               
            }
        }

        protected void SendEmailConfirmationToken(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindByEmail(Email.Text);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    string code = manager.GenerateEmailConfirmationToken(user.Id);
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    commonLogic.SendRegisterConfirmationEmail(manager, user, callbackUrl);

                    FailureText.Text = string.Format(GlobalConstants.EmailRegistrationSendOK, Email.Text);
                    ErrorMessage.Visible = true;                    
                }
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();                

                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true

                //find user by eMail 
                var user = manager.FindByEmail(Email.Text);

                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        FailureText.Text = "Neuspešný pokus o prihlásenie, najskôr musíte potvrdiť email.";
                        ErrorMessage.Visible = true;
                        ResendConfirm.Visible = true;                        
                    }
                    else
                    {                        
                        var result = signinManager.PasswordSignIn(user.UserName, Password.Text, RememberMe.Checked, shouldLockout: false);                                                

                        switch (result)
                        {
                            case SignInStatus.Success:
                                //FormsAuthentication.SetAuthCookie(user.UserName, RememberMe.Checked);
                                //Session[GlobalConstants.UserNick] = !string.IsNullOrEmpty(user.ScoutNickName) ? user.ScoutNickName : user.FirstName;

                                if (Properties.Settings.Default.EnableStatistic)
                                {
                                    using (var _db = new Kronika106DBContext())
                                    {
                                        ApplicationUser aUser = _db.Users.FirstOrDefault(u => u.UserName == user.UserName);
                                        if (aUser != null)
                                        {
                                            aUser.LastLogInUTC = DateTime.UtcNow;
                                            if (aUser.LoginCount.HasValue)
                                                aUser.LoginCount++;
                                            else
                                                aUser.LoginCount = 1;
                                            _db.SaveChanges();
                                        }

                                    }
                                }

                                    string returnUrl = Request.QueryString["ReturnUrl"];
                                if (string.IsNullOrEmpty(returnUrl))
                                {
                                    returnUrl = (string)Session[GlobalConstants.RedirectURLKey];
                                }
                                
                                IdentityHelper.RedirectToReturnUrl(returnUrl, Response);

                                break;
                            case SignInStatus.LockedOut:
                                Response.Redirect("/Account/Lockout");
                                break;
                            case SignInStatus.RequiresVerification:
                                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                                Request.QueryString["ReturnUrl"],
                                                                RememberMe.Checked),
                                                  true);
                                break;
                            case SignInStatus.Failure:
                            default:
                                FailureText.Text = "Chybný pokus pre prihlásenie";
                                ErrorMessage.Visible = true;
                                break;
                        }
                    }
                }
                else
                {
                    FailureText.Text = string.Format("Užívateľ neexistuje, prosím zeregistrujete sa");
                    ErrorMessage.Visible = true;                
                }               
            }
        }

        protected void RememberMe_CheckedChanged(object sender, EventArgs e)
        {                                 
        }
    }
}