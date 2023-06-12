using Kronika106.Logic;
using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Kronika106
{
    public partial class Gallery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Navigator.GenerateNavigation(Page.Master);
            }
        }

        public List<Kronika106.FileSystemModel.Year> LoadYears()
        {
            if (!IsPostBack)
            {
                List<Kronika106.FileSystemModel.Year> lstRoky = new List<FileSystemModel.Year>();
                var directiories = Directory.EnumerateDirectories(Server.MapPath(GlobalConstants.PthFileSystemRoot));
                using (var _db = new Kronika106DBContext())
                {
                    foreach (var dir in directiories)
                    {
                        Kronika106.FileSystemModel.Year yr = new FileSystemModel.Year();
                        yr.Rok = Path.GetFileName(dir);

                        yr.PocetKomentarov = _db.Forum.Count(c => c.EventId.StartsWith(yr.Rok));

                        string descPath = Path.Combine(dir, GlobalConstants.fnRokPopisShort);
                        if (File.Exists(descPath))
                        {
                            yr.Popis = File.ReadAllText(descPath);
                        }
                        yr.PathFotka = string.Format("{0}/{1}/{2}", GlobalConstants.PthFileSystemRoot, yr.Rok, GlobalConstants.fnRokFotka);
                        lstRoky.Add(yr);
                    }
                }
                return lstRoky;
            }
            return null;
        }
    }
}