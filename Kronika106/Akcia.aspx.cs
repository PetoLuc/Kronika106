using Kronika106.Logic;
using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web.ModelBinding;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Threading;

namespace Kronika106
{
    public partial class Akcia : EventPagesBase
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
				Page.Title = base.PageTitleBase + " Galéria";
				header.InnerText = EventIdParams[1];
			}
		}

        public List<Kronika106.FileSystemModel.Photo> AkciePhotosForEvent()
        {            
            List<Kronika106.FileSystemModel.Photo> lstPhotos = new List<FileSystemModel.Photo>();                        
            var photos = Directory.GetFiles(FileSystemPath, "*.jpg", SearchOption.TopDirectoryOnly);
            int index = 0;
            foreach (var photo in photos)
            {
                if (photo.Contains(GlobalConstants.fnAkciaFotka))
                {
                    continue;
                }
                Kronika106.FileSystemModel.Photo rcPhoto = new FileSystemModel.Photo();

                rcPhoto.PathSliderPhoto = string.Format("{0}/{1}", RelativePath, Path.GetFileName(photo));
                rcPhoto.PathThumbialPhoto = string.Format("{0}/{1}/{2}", RelativePath, GlobalConstants.PthThumb, Path.GetFileName(photo));
                rcPhoto.PhotoIndex = index++;
                lstPhotos.Add(rcPhoto);
            }
            return lstPhotos;
        }
    }
}