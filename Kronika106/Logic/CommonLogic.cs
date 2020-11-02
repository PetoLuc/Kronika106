using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Kronika106.Logic
{
    public class CommonLogic
    {
        private static System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("sk-SK");
        public bool IsDuplicateNick(string scoutNick, string userIDMail, out string error)
        {
            error = string.Empty;
            using (var _db = new Kronika106DBContext())
            {
                if (!string.IsNullOrEmpty(scoutNick))
                {
                    var eUsr = _db.Users.FirstOrDefault(u => u.ScoutNickName.ToLower() == scoutNick.ToLower());
                    if (eUsr != null)
                    {
                        error = string.Format("Užívateľ so skautskou prezývkou: {0} je už registrovaný, zadajte inú prezývku napr: {0}zDetvy :-)", scoutNick);
                        //nerobit redirect zostavat na stránke
                        return true;
                    }
                    eUsr = _db.Users.FirstOrDefault(u => u.Id == userIDMail);
                    if (eUsr != null)
                    {
                        error = string.Format("Užívateľ s mail adresou {0} je už registrovaný.", userIDMail);
                        //nerobit redirect zostavat na stránke
                        return true;
                    }
                }
            }
            return false;
        }

        public static void GenerateNavigation(MasterPage masterSite, params string[] args)
        {
            if (args == null || args.Length == 0)
                return;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            { sb.Append(args[i]); }
            ((SiteMaster)masterSite).PageNavigator.Visible = true;
            ((SiteMaster)masterSite).PageNavigator.InnerHtml= sb.ToString(); 
        }

        public static string ToSKDateTime(DateTime utcDateTime)
        {            
            return utcDateTime.ToString("g", ci);
        }

        public void SendRegisterConfirmationEmail(ApplicationUserManager manager, ApplicationUser user, string callbackUrl)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Prosím potvrďte registráciu kliknutím <a href=\"{callbackUrl}\"><b> SEM</b></a><br>");
            builder.AppendLine("Kronika 106 zboru Akataleptik Detva");            
            manager.SendEmailAsync(user.Id, "Potvrdenie registrácie pre Kroniku 106", builder.ToString());
        }

    }
}