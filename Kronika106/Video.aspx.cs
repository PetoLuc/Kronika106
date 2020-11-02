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
    public partial class Video : EventPagesBase
    {
        protected override int NumberOfParams
        {
            get
            {
                return 4;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = string.Format("{0} {1} - {2}", EventIdParams[0], EventIdParams[1], EventIdParams[3]);
            }
        }

        public FileSystemModel.Video GetVideoDetail()
        {
            
            if (Directory.Exists(FileSystemPath))
            {
                if (EventIdParams.Length > 3)
                {
                    eventName.InnerText = string.Format("{0} - {1}", EventIdParams[1], EventIdParams[3]);                    
                }                

                FileSystemModel.Video video = new FileSystemModel.Video();
                //popis
                string descPath = Path.Combine(FileSystemPath, GlobalConstants.fnVideoPopis);
                if (File.Exists(descPath))
                {
                    video.Popis = File.ReadAllText(descPath);
                }
                else
                {
                    video.Popis = string.Empty;
                }
                VideoPopis.InnerText = video.Popis;

                //poster
                string local = Server.MapPath(Request.ApplicationPath);
                string posterPath = Path.Combine(FileSystemPath, GlobalConstants.fnVideoPoster);
                //video.Poster = string.Empty;
                //if (File.Exists(posterPath))
                //{                   
                //    posterPath = posterPath.Replace(local, "");
                //    video.PathVideoPoster = posterPath;
                //}

                //videa
                string[] files =  Directory.GetFiles(FileSystemPath);
                if (files != null && files.Length > 0)
                {
                    video.MP4 = files.FirstOrDefault(p => Path.GetFileName(p).Equals(GlobalConstants.fnVideoMP4, StringComparison.CurrentCultureIgnoreCase))?.Replace(local, @"\");
                    video.OGV = files.FirstOrDefault(p => Path.GetFileName(p).Equals(GlobalConstants.fnVideoOGV, StringComparison.CurrentCultureIgnoreCase))?.Replace(local, @"\");
                    video.WEBM = files.FirstOrDefault(p => Path.GetFileName(p).Equals(GlobalConstants.fnVideoWEBM, StringComparison.CurrentCultureIgnoreCase))?.Replace(local, @"\");
                    video.Poster = files.FirstOrDefault(p => Path.GetFileName(p).Equals(GlobalConstants.fnVideoPoster, StringComparison.CurrentCultureIgnoreCase))?.Replace(local, @"\");
                }
                return video;
            }
            return null;
            
        }
    }
}