using Kronika106.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Kronika106.ForumControll;

namespace Kronika106
{
    public partial class SearchResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //kontrola ci je user prihlaseny            
            Session[GlobalConstants.RedirectURLKey] = HttpContext.Current.Request.Url.PathAndQuery;
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect(GlobalConstants.urlForbidden, true);
                return;
            }            
            if (!IsPostBack)
            {
                string searchString = HttpUtility.UrlDecode(Request.QueryString["search"]);
                Page.Title = "Vyhľadávanie: "+ searchString;
                //zdvojena kontrola ak by niekto poslal link
                if (string.IsNullOrWhiteSpace(searchString))
                {
                    title.InnerText = "Pre vyhľadávanie musíš zadať aspoň jeden znak.";
                    return;
                }
                
                

                List<string> lstAll = new List<string>();                                                                
                lstAll = searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (lstAll.All(s => s.Length < 3))
                {
                    //vo vyhladavani musi byt aspon jeden retazec dlhsi ako 2 znaky - obmedzenie kvoli slabemu vykonu prividera
                    title.InnerText = "Aspoň jendo slovo musí byť dlhšie ako 2 znaky";
                    return;
                }


                Logic.Search search = new Logic.Search(lstAll);
                List<PageContent> searchResult = search.SearchAll();
                if (searchResult!=null && searchResult.Count > 0)
                {
                    title.InnerText = $"Nájdených {searchResult.Count()} výskedkov pre: \"{searchString}\"";
                 
                }
                else
                {
                    //nenasli za ziadne zaznamy
                    title.InnerText = $"\"{searchString}\" sa v kronike nenašlo..";
                }
                ForumControll.ContentList = searchResult;
            }
        }
        
    }
}