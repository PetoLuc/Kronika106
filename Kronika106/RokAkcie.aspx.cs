using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Kronika106.Logic;
using System.Threading;
using Kronika106.Models;

namespace Kronika106
{
	public partial class RokAkcie : System.Web.UI.Page
	{
        private string yearPath;

        protected void Page_Load(object sender, EventArgs e)
		{
                         
                if (!IsPostBack)
                {
                    Session[GlobalConstants.RedirectURLKey] = HttpContext.Current.Request.Url.PathAndQuery;
                    if (Request.QueryString["ID"] == null)
                    {
                        Response.Redirect(GlobalConstants.urlDefault, true);
                        return;
                    }
                    
                    //kontrola na existenciu adresara z requestu
                    string yearId = Request.QueryString["ID"];
                    yearPath = string.Format("{0}/{1}", GlobalConstants.PthFileSystemRoot, yearId);

                    if (!System.IO.Directory.Exists(Server.MapPath(yearPath)))
                    {
                        Response.Redirect(GlobalConstants.urlDefault);
                    }
                    Navigator.GenerateNavigation(Page.Master);
                    this.Title = yearId;
                }                                       
        }

        public List<Kronika106.FileSystemModel.Akcia> AkcieGetForYear([QueryString("ID")] string yearId)
        {
            if (!IsPostBack)
            {
                List<Kronika106.FileSystemModel.Akcia> lstAkcie = new List<FileSystemModel.Akcia>();
                if (!string.IsNullOrEmpty(yearId))
                {
                    var directiories = Directory.EnumerateDirectories(Server.MapPath(yearPath));
                    using (var _db = new Kronika106DBContext())
                    {
                        foreach (var dir in directiories)
                        {
                            Kronika106.FileSystemModel.Akcia akcia = new FileSystemModel.Akcia();
                            akcia.Nazov = Path.GetFileName(dir);

                            string dbKey = $"{yearId}/{akcia.Nazov}";
                            akcia.PocetKomentarov = _db.Forum.Count(c => c.EventId.StartsWith(dbKey));


                            string descPath = Path.Combine(dir, GlobalConstants.fnAkciaPopisShort);
                            if (File.Exists(descPath))
                            {
                                akcia.Popis = File.ReadAllText(descPath);
                            }
                            else
                            {
                                akcia.Popis = string.Empty;
                            }
                            akcia.PathFotka = string.Format("{0}/{1}/{2}/{3}", GlobalConstants.PthFileSystemRoot, yearId, akcia.Nazov, GlobalConstants.fnAkciaFotka);
                            akcia.URL = Page.Server.UrlEncode(string.Format("{0}/{1}", yearId, akcia.Nazov));
                            lstAkcie.Add(akcia);
                        }
                    }
                    return lstAkcie;
                }
            }
            return null;
        }
	

        // The id parameter should match the DataKeyNames value set on the control
        // or be decorated with a value provider attribute, e.g. [QueryString]int id
        public Kronika106.FileSystemModel.Year yearDetail_GetItem([QueryString("ID")] string yearId)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(yearId))
                {
                    Kronika106.FileSystemModel.Year yr = new FileSystemModel.Year();
                    string yearPath = string.Format("{0}/{1}", GlobalConstants.PthFileSystemRoot, yearId);
                    string descPath = Path.Combine(Server.MapPath(yearPath), GlobalConstants.fnRokPopis);
                    yr.Rok = yearId;
                    if (File.Exists(descPath))
                    {
                        yr.Popis = File.ReadAllText(descPath);
                    }
                    else
                    {
                        yr.Popis = GlobalConstants.EmptyPopis;
                    }
                    return yr;
                }
            }
            return null;
        }
    }
}