using ICSharpCode.SharpZipLib.Zip;
using Kronika106.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xceed.Words.NET;
using static Kronika106.ForumControll;

namespace Kronika106.Admin
{
    public partial class WordGenerator : System.Web.UI.Page
    {


        static Formatting formatYear = new Formatting() { FontFamily = new Font("Calibri (Základný text)"), Bold = true, Size = 20 };
        static Formatting formatAkcia = new Formatting() { FontFamily = new Font("Calibri (Základný text)"), Bold = true, Size = 14 };
        static Formatting formatText = new Formatting() { FontFamily = new Font("Calibri (Základný text)"), Size = 11 };
        static Formatting formatNick = new Formatting() { FontFamily = new Font("Calibri (Základný text)"), Size = 11, Bold = true, Italic = true};
        static Formatting formatComment = new Formatting() { FontFamily = new Font("Calibri (Základný text)"), Size = 11, Italic = true };
        private List<PageContent> contentList = null;
        DocX doc = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Context.User.Identity.IsAuthenticated || !HttpContext.Current.User.IsInRole(GlobalConstants.RoleAdmin))
            //{
            //    Response.Redirect(GlobalConstants.urlDefault);
            //}
            //ErrorMessage.Visible = false;
            //InfoMessage.Visible = false;
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {                        
            const int yeH = 266;
            const int yeW = 472;
            const double eventPhotoScalePercentage = 0.7;   // /100
            
            using (var _db = new Kronika106.Models.Kronika106DBContext())
            {
                contentList = (from EventComments comment in _db.Forum                                                                
                                 //(comment.EventId.ToLower() == trueEvetId && ((forumType == ForumType.Event && comment.IsEvent) || (forumType == ForumType.EventPhotoGallery && comment.IsPhoto && comment.ThumbPath.Contains(photoName))))
                               orderby comment.ID ascending
                               select new PageContent { ID = comment.ID, CreatedUTC = comment.CreatedUTC, EventId = comment.EventId, RootID = comment.RootID, ThumbPath = comment.ThumbPath, ScoutNickName = comment.ApplicationUser.ScoutNickName, NickName = comment.ApplicationUser.NickName, Comment = comment.Comment, IsVideo = comment.IsVideo, IsEvent = comment.IsEvent, IsPhoto = comment.IsPhoto, UserID = comment.ApplicationUser.Id, UserName = comment.ApplicationUser.UserName/*, VideoPosition= comment.VideoPosition */}).ToList();
            }
            if (contentList == null || contentList.Count == 0)
            {                
                return;                
            }

            string now = DateTime.Now.ToString("yyyyMMddHHmmss");

            string pthAppData = Server.MapPath("..\\App_Data");                   
            string pthAllPhotos = Server.MapPath("..\\AllPhotos");

            DirectoryInfo dAllPhotosInfo = new DirectoryInfo(pthAllPhotos);
            List<string> lstDocFiles = new List<string>();
            foreach (var dYear in dAllPhotosInfo.GetDirectories())
            {                
                //rok                                
                //DateTime.Now.ToString("yyyyMMddHHmmss")
                string fileName = $"kronika106_{dYear.Name}_{now}.docx";
                string fileFullPath = System.IO.Path.Combine(pthAppData, fileName);
                doc = DocX.Create(fileFullPath);
                doc.InsertSection();
                doc.InsertParagraph(dYear.Name, false, formatYear).Heading(HeadingType.Heading1).SpacingAfter(25d).Alignment = Alignment.center;
                var yImage = doc.AddImage(Path.Combine(dYear.FullName, GlobalConstants.fnRokFotka));
                var pYImageYear = doc.InsertParagraph();
                pYImageYear.AppendPicture(yImage.CreatePicture(yeH, yeW)).SpacingAfter(20d).Alignment = Alignment.center;


                var yPopisPath = Path.Combine(dYear.FullName, GlobalConstants.fnRokPopis);
                if (System.IO.File.Exists(yPopisPath))
                {
                    doc.InsertParagraph(System.IO.File.ReadAllText(yPopisPath), false, formatText).SpacingAfter(20d).Alignment = Alignment.both;
                }
                doc.InsertSectionPageBreak(); 

                //toto rozdelit na zanostatny paragraf pre obrazok a pre text        
                foreach (var dAkcia in dYear.GetDirectories())
                {
                    doc.InsertParagraph(dAkcia.Name, false, formatAkcia).Heading(HeadingType.Heading2).SpacingAfter(15d).Alignment = Alignment.center;
                    string eventImagePath = Path.Combine(dAkcia.FullName, GlobalConstants.fnAkciaFotka);
                    if (File.Exists(eventImagePath))
                    {
                        var pImageEvent = doc.InsertParagraph();
                        var yImageEvent = doc.AddImage(eventImagePath);
                        pImageEvent.AppendPicture(yImageEvent.CreatePicture(yeH, yeW)).SpacingAfter(15d).Alignment = Alignment.center;
                    }

                    string eventDescriptionPath = Path.Combine(dAkcia.FullName, GlobalConstants.fnAkciaPopis);
                    if (File.Exists(eventDescriptionPath))
                    {
                        doc.InsertParagraph(File.ReadAllText(eventDescriptionPath), false, formatText).SpacingAfter(15d).Alignment = Alignment.both;
                    }

                    string dbEventId = dAkcia.FullName.Replace(pthAllPhotos, "").TrimStart('\\').Replace("\\", "/");
                    commentTree(dbEventId, null, ref doc, ForumType.Event);

                    //precitat fotky:
                    foreach (var ePhoto in dAkcia.GetFiles("*.jpg"))
                    {
                        //vynechame uvodny obrazok
                        if (ePhoto.Name.Equals(GlobalConstants.fnAkciaFotka)) continue;
                        var pPhotoEvent = doc.InsertParagraph();                        
                        var yPhotoEvent = doc.AddImage(ePhoto.FullName);
                        var pct = yPhotoEvent.CreatePicture();
                        pct.Width = (int)Math.Round(pct.Width * eventPhotoScalePercentage, 0);
                        pct.Height = (int)Math.Round(pct.Height * eventPhotoScalePercentage, 0);

                        pPhotoEvent.AppendPicture(pct).SpacingAfter(5d).SpacingBefore(8d).Alignment = Alignment.center;

                        //tu najst podla nazvu akcie a fotky komnetare  a doplmnit -usera 
                        
                        commentTree(dbEventId, ePhoto.Name, ref doc, ForumType.EventPhotoGallery);
                    }
                    doc.InsertSectionPageBreak();                    
                }
                doc.Save();
                lstDocFiles.Add(fileFullPath);                
            }
            string zipFileName = Path.Combine(pthAppData, $"Kronika2Word_{now}.zip");
            createZipFile(zipFileName, lstDocFiles);

            //mama zip zmazeme
            foreach (var file in lstDocFiles)
            {
                System.IO.File.Delete(file);
            }

            //upload na klienta
            FileInfo fInfo = new FileInfo(zipFileName);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fInfo.Name);
            Response.AddHeader("Content-Length", fInfo.Length.ToString());
            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            Response.Flush();
            Response.TransmitFile(fInfo.FullName);
            Response.End();

            foreach (var file in lstDocFiles)
            {
                System.IO.File.Delete(file);
            }

        }

        private void createZipFile(string fileName, IEnumerable<string> files)
        {
            // Create and open a new ZIP file
            using (ZipOutputStream s = new ZipOutputStream(File.Create(fileName)))
            {

                s.SetLevel(0); // 0 - store only to 9 - means best compression
                byte[] buffer = new byte[4096];
                foreach (string file in files)
                {
                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.
                    var entry = new ZipEntry(Path.GetFileName(file));

                    // Setup the entry data as required.

                    // Crc and size are handled by the library for seakable streams
                    // so no need to do them here.

                    // Could also use the last write time or similar for the file.
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);

                    using (FileStream fs = File.OpenRead(file))
                    {

                        // Using a fixed size buffer here makes no noticeable difference for output
                        // but keeps a lid on memory usage.
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
        }


        void commentTree(string trueEvetId, string photoName, ref DocX outDoc, ForumType forumType)
        {
            //hladnaie rootu akcie / fotky
            var sublist = contentList.Where(c => c.EventId == trueEvetId && c.RootID==null && ((forumType== ForumType.Event && c.IsEvent==true) || (forumType == ForumType.EventPhotoGallery && c.IsPhoto == true && c.ThumbPath.Contains(photoName))));            
            foreach (var comment in sublist.Where(c => c.RootID == null)) //iterujeme len rooty
            {                
                addComment(comment);
                docTree(comment,1);
                doc.InsertParagraph();
            }
        }        

        private  void docTree(PageContent comment, int tabCount)
        {
            foreach (var reply in contentList.Where(c => c.RootID == comment.ID))
            {                
                addComment(reply, tabCount);
                docTree(reply, tabCount+1);
            }
            
        }
        private void addComment(PageContent comment,int? tabsCount = null)
        {            
            string nick = comment.ScoutNickName != null ? comment.ScoutNickName : comment.NickName;
            var commentParagraph = doc.InsertParagraph();
            if (tabsCount.HasValue)
            {
                commentParagraph.IndentationBefore = tabsCount.Value;
            }
            commentParagraph.Alignment = Alignment.both;            
            commentParagraph.Append($"{nick}", formatNick);
            commentParagraph.Append($"{ " - "}{comment.Comment}", formatComment);
            
        }

    }
}