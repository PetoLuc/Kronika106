using Kronika106.Logic;
using Kronika106.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Threading;

namespace Kronika106
{	
    public partial class AkciaPopis : EventPagesBase
    {        

        protected override int NumberOfParams
        {
            get
            {
                return 2;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {                        
                if (!IsPostBack)
                {
                    Page.Title = base.PageTitleBase + " Popis";
                    header.InnerText = base.StrPageHeader;                    

                    //popis akcie
                    string descPath = Path.Combine(FileSystemPath, GlobalConstants.fnAkciaPopis);
                    EventPopis.InnerHtml = GlobalConstants.EmptyPopis;
                    if (File.Exists(descPath))
                    {
                        EventPopis.InnerHtml = File.ReadAllText(descPath);
                    }

                    //mapa akcie
                    string mapPath = Path.Combine(FileSystemPath, GlobalConstants.fnMapa);
                    if (File.Exists(mapPath))
                    {
                        idMapa.InnerHtml = File.ReadAllText(mapPath);
                    }

					//menu					
					//kontrola ci existuje video
					string pthVideo = Path.Combine(FileSystemPath, GlobalConstants.PthVideoRoot);
					if (Directory.Exists(pthVideo))
					{
						string[] content = Directory.GetFiles(pthVideo, "*", SearchOption.AllDirectories);
						if (content.Length > 0)
						{
							btnLinkToVideo.Visible = true;
                        boxVideo.Visible = true;
						}                        
					}

					var photos = Directory.GetFiles(FileSystemPath, "*.jpg", SearchOption.TopDirectoryOnly).Where(name=> !name.EndsWith(GlobalConstants.fnAkciaFotka));
                    
					if (photos != null && photos.ToList().Count > 0)
					{
					    btnLinkGallery.Visible = true;
                        boxGallery.Visible = true;              
					}

					if (btnLinkGallery.Visible && btnLinkToVideo.Visible)
						colLeftOrder.Attributes.Add("class", "col-md-4");
					else
					{
						colLeftOrder.Attributes.Add("class", "col-md-5");
					}                                                         
                }           
        }

        protected void linkToGallery_Click(object sender, EventArgs e)
        {
            Response.Redirect("Akcia.aspx?ID=" + Request["ID"]);
        }

        protected void linkToVideoGallery_Click(object sender, EventArgs e)
        {
            string relPath = Path.Combine(GlobalConstants.PthFileSystemRoot, Server.UrlDecode(Request.QueryString["ID"]), GlobalConstants.PthVideoRoot);
            string[] videoDirs = Directory.GetDirectories(Server.MapPath(relPath));
            if (videoDirs.Length > 1)
            {
                Response.Redirect("VideoList.aspx?ID=" + Request["ID"]);
            }
            else
            {
                string request = Page.Server.UrlEncode(string.Format("{0}/{1}/{2}", Request.QueryString["ID"], GlobalConstants.fnVideoDir, Path.GetFileName(videoDirs[0])));
                Response.Redirect(string.Format("Video.aspx?ID={0}", request));
            }
        }
    }
}