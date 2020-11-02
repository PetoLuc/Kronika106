using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Linq;
using Kronika106.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Globalization;
using Microsoft.Owin.Security;

namespace Kronika106
{
	public partial class SiteMaster : MasterPage
	{
		private const string AntiXsrfTokenKey = "__AntiXsrfToken";
		private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
		private string _antiXsrfTokenValue;        

		public System.Web.UI.HtmlControls.HtmlGenericControl MasterPageNavigator
		{
			get { return this.PageNavigator; }
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			// The code below helps to protect against XSRF attacks
			var requestCookie = Request.Cookies[AntiXsrfTokenKey];
			Guid requestCookieGuidValue;
			if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
			{
				// Use the Anti-XSRF token from the cookie
				_antiXsrfTokenValue = requestCookie.Value;
				Page.ViewStateUserKey = _antiXsrfTokenValue;
			}
			else
			{
				// Generate a new Anti-XSRF token and save to the cookie
				_antiXsrfTokenValue = Guid.NewGuid().ToString("N");
				Page.ViewStateUserKey = _antiXsrfTokenValue;

				var responseCookie = new HttpCookie(AntiXsrfTokenKey)
				{
					HttpOnly = true,
					Value = _antiXsrfTokenValue
				};
				if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
				{
					responseCookie.Secure = true;
				}
				Response.Cookies.Set(responseCookie);
			}

			Page.PreLoad += master_Page_PreLoad;                        
		}

		protected void master_Page_PreLoad(object sender, EventArgs e)
		{            
			if (!IsPostBack)
			{
				// Set Anti-XSRF token
				ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
				ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
			}
			else
			{
				// Validate the Anti-XSRF token
				if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
					|| (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
				{

					//todo tato chyba nastavala ked sa nedokoncilo prihlasenie cez FB
					// throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            adminMenu.Visible = false;
            if (Context.User.Identity.IsAuthenticated)
			{
				string userId = Context.User.Identity.Name;
				if (string.IsNullOrEmpty(userId))
				{
					LogOutAll();										
				}

                if (HttpContext.Current.User.IsInRole(GlobalConstants.RoleAdmin))
                {
                    adminMenu.Visible = true;                    
                }                
			}
		}

		protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
		{
            adminMenu.Visible = false;
            if (Properties.Settings.Default.EnableStatistic)
            {
                using (var _db = new Kronika106DBContext())
                {
                    string userId = Context.User.Identity.Name;
                    ApplicationUser aUser = _db.Users.FirstOrDefault(u => u.UserName == userId);
                    if (aUser != null)
                    {
                        aUser.LastLogOffUTC = DateTime.UtcNow;                        
                        _db.SaveChanges();
                    }

                }
            }
            LogOutAll();

		}

		private void LogOutAll()
		{
			Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.TwoFactorCookie);
			Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
			Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);
			FormsAuthentication.SignOut();
            Session.Remove(GlobalConstants.UserNick);
            Session.Clear();            
			Session.Abandon();
			//toto po abandon byt asi nemusi ale pre istotu...
			
		}

		private class UserNick
		{
			public string FirstName { get; set; }
			public string ScoutNickName { get; set; }
		}


        /// <summary>
        /// vrati skautsku prezyvku alebo nick
        /// </summary>
        /// <returns></returns>
        protected string GetUserNick()
        {
            string userId = Context.User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                LogOutAll();
                Response.Redirect(GlobalConstants.urlDefault);
                Session[GlobalConstants.UserNick] = null;
                return null;
            }
            if (!IsPostBack)
            {
                if (Context.User.Identity.IsAuthenticated)
                {

                    if (Session[GlobalConstants.UserNick] == null)
                    {

                        using (var _db = new Kronika106.Models.Kronika106DBContext())
                        {
                            IEnumerable<UserNick> query = from ApplicationUser usr in _db.Users where usr.UserName == userId select new UserNick { FirstName = usr.FirstName, ScoutNickName = usr.ScoutNickName };
                            UserNick userData = query?.FirstOrDefault();
                            if (userData == null)
                            {
                                LogOutAll();
                                Response.Redirect(GlobalConstants.urlDefault);
                                Session[GlobalConstants.UserNick] = null;
                                return null;
                            }
                            if (!string.IsNullOrEmpty(userData?.ScoutNickName))
                            {
                                Session.Add(GlobalConstants.UserNick, userData?.ScoutNickName);
                            }
                            else {
                                Session.Add(GlobalConstants.UserNick, userData?.FirstName);
                            }
                        }
                    }
                }
                else
                {
                    Session.Remove(GlobalConstants.UserNick);
                }
            }
            string val = (string)Session[GlobalConstants.UserNick];
            return val;
        }        
    }
}