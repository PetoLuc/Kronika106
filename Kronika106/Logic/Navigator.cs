using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Kronika106.Logic
{
    public class Navigator
    {
        const string navHome = "<li><a href=\"Default\"><i class=\"fa fa-home\"></i></a></li>";
        //public const string navHomeActive = "<li class=\"active\"><i class=\"fa fa-home\"></i></li>";
        const string navGallery = "<li><a href=\"Gallery\">Kronika</a></li>";
        const string navGalleryActive = "<li class=\"active\"><a>Kronika</a></li>";
        const string navYear = "<li><a href=\"RokAkcie.aspx?ID={0}\">{1}</a></li>";
        const string navYearActive = "<li class=\"active\"><a>{0}</a></li>";
        const string navEvent = "<li><a href=\"AkciaPopis.aspx?{0}\">{1}</a></li>";
        const string navEventActive = "<li class=\"active\"><a>{0}</a></li>";
        const string navPhotosActive = "<li class=\"active\"><a>Galéria</a></li>";
        const string navEventVideoList = "<li><a href=\"VideoList.aspx?ID={0}\">zoznam videí</a></li>";
        const string navEventVideoListActive = "<li class=\"active\"><a>zoznam videí</a></li>";
        const string navEventVideoActive = "<li class=\"active\"><a>{0}</a></li>";   //nazov videa


        public static void GenerateNavigation(MasterPage masterSite)
        {
            ((SiteMaster)masterSite).PageNavigator.Visible = true;
            
            string request = masterSite.Request.QueryString.ToString();

            //if (string.IsNullOrEmpty(request))
            //    return null;

            StringBuilder navigatorHtml = new StringBuilder();
            navigatorHtml.AppendFormat(navHome);
            string[] requestParams = new string[] { };

            if (!string.IsNullOrEmpty(request))
            {
                requestParams = QueryStringHelper.GetIdFromRequest(masterSite.Request).Split(GlobalConstants.EventIdSeparator, StringSplitOptions.RemoveEmptyEntries);//  masterSite.Server.UrlDecode(request).Replace("ID=", "").Split(GlobalConstants.EventIdSeparator, StringSplitOptions.RemoveEmptyEntries);
            }

            switch (requestParams.Length)
            {
                case 0:
                    //geleria
                    
                    navigatorHtml.AppendFormat(navGalleryActive);
                    break;
                case 1:
                    //rok -akcie                    
                    navigatorHtml.AppendFormat(navGallery);
                    navigatorHtml.AppendFormat(navYearActive, requestParams[0]);
                    break;
                case 2:
                case 4:                
                    //akcia popis //akcia fotogaleria                    
                    string pageName = Path.GetFileName(masterSite.Request.PhysicalPath);                    
                    navigatorHtml.AppendFormat(navGallery);
                    navigatorHtml.AppendFormat(navYear, masterSite.Server.UrlEncode(requestParams[0]), requestParams[0]);
                    switch (pageName.ToLower())
                    {
                        case "akcia":
                            navigatorHtml.AppendFormat(navEvent, request, requestParams[1]);
                            navigatorHtml.AppendFormat(navPhotosActive, requestParams[1]);
                            break;
                        case "videolist":
                            navigatorHtml.AppendFormat(navEvent, request, requestParams[1]);                            
                            navigatorHtml.AppendFormat(navEventVideoListActive);
                            break;
                        case "video":                            
                            if (requestParams.Length == 4)
                            {
                                string eventUrl = masterSite.Server.UrlEncode(string.Format("{0}/{1}", requestParams[0], requestParams[1]));
                                navigatorHtml.AppendFormat(navEvent,"ID="+ eventUrl, requestParams[1]);
                                navigatorHtml.AppendFormat(navEventVideoList, eventUrl);
                                navigatorHtml.AppendFormat(navEventVideoActive, requestParams[3]);
                            }
                            break;
                        default:
                            navigatorHtml.AppendFormat(navEventActive, requestParams[1]);
                            break;
                    }                                        
                    break;                
                
            }
            ((SiteMaster)masterSite).PageNavigator.InnerHtml = navigatorHtml.ToString();
        }



    }
}