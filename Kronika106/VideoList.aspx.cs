using Kronika106.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kronika106
{
    public partial class VideoList : EventPagesBase
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
                Page.Title = string.Format("{0} {1} - zoznam videí", EventIdParams[0], EventIdParams[1]);
                VideoListTitle.InnerText = StrPageHeader;
            }
        }

        public List<FileSystemModel.Video> GetVideosForAkcie()
        {            


            List<FileSystemModel.Video> lstRes = new List<FileSystemModel.Video>();
            string pthVideo = Path.Combine(FileSystemPath, GlobalConstants.PthVideoRoot);
            if (!Directory.Exists(pthVideo))
            {
                Response.Redirect(GlobalConstants.urlDefault, true);
            }
            string[] videoRoots = Directory.GetDirectories(pthVideo, "*", SearchOption.TopDirectoryOnly);
            if (videoRoots.Length == 0)
            {
                Response.Redirect(GlobalConstants.urlDefault, true);
            }
            foreach (string videoRoot in videoRoots)
            {
                FileSystemModel.Video video = new FileSystemModel.Video();
                video.Nazov = Path.GetFileName(videoRoot);
                video.DetailURL = Page.Server.UrlEncode(string.Format("{0}/{1}/{2}", Request.QueryString["ID"], GlobalConstants.fnVideoDir, video.Nazov));

                string descPath = Path.Combine(videoRoot, GlobalConstants.fnVideoPopisShort);
                if (File.Exists(descPath))
                {
                    video.Popis = File.ReadAllText(descPath);
                }
                else
                {
                    video.Popis = string.Empty;
                }

                string posterPath = Path.Combine(videoRoot, GlobalConstants.fnVideoPoster);
                video.Poster = string.Empty;
                if (File.Exists(posterPath))
                {
                    string local = Server.MapPath(Request.ApplicationPath);
                    posterPath = posterPath.Replace(local, "");
                    video.Poster = posterPath;
                }

                lstRes.Add(video);
            }
            return lstRes;
        }
    }
}