using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kronika106.Models;
using Kronika106.FileSystemModel;
using System.IO;
using System.Web.UI;
using System.Collections;

namespace Kronika106.Logic
{

    public class SearchDictionary : Dictionary<string, bool>
    {
        public SearchDictionary(List<string> lstSearchTerms)
        {
            foreach (string term in lstSearchTerms)
            {
                if (!ContainsKey(term))
                {
                    Add(term.ToLower(), false);
                }
            }
        }
        public void ResetMatches()
        {
            foreach (string key in this.Keys.ToList())
            {                
                this[key] = false;
            }
        }

        
    }
    public class Search:Page
    {
        private char[] splitters = new char[] { ' '/*, ',', '.', '?', '!', ':', '-', '/', ';', '+', '\\', ')', '(', '[', ']', '{', '}', '~'*/ };
        private char[] trimChars = new char[] {',', '.', '?', '!', ':', '-', '/', ';', '+', '\\', ')', '(', '[', ']', '{', '}', '~'};
        private const string matchHighliht = "<b style=\"color: darkred; text-transform:uppercase\">{0}</b>";

        private SearchDictionary dctSearchTerms;


        public Search(List<string> lstSearchTerms)
        {
            //od najdlhsih
            if (lstSearchTerms==null)
            {
                throw new ArgumentNullException("lstSearchTerms");
            }
            lstSearchTerms = lstSearchTerms.OrderByDescending(s => s.Length).ToList();
            dctSearchTerms = new SearchDictionary(lstSearchTerms);
        }


        public List<ForumControll.PageContent>  SearchAll()
        {
            List<ForumControll.PageContent> resList = new List<ForumControll.PageContent>();
            
            resList.AddRange(SearchFileSystem());
            resList.AddRange(SearchComments());
            return resList;

        }


        private List<ForumControll.PageContent> SearchComments()
        {
            List<ForumControll.PageContent> dbResult = null;
            List<ForumControll.PageContent> commentResult = new List<ForumControll.PageContent>();
            //hladanie v DB
            using (var _db = new Kronika106DBContext())
            {
                dbResult = (from EventComments events in _db.Forum
                            where dctSearchTerms.Keys.Any(x => events.Comment.Contains(x))
                            orderby events.CreatedUTC descending
                            select new ForumControll.PageContent { ID = events.ID, NickName = events.ApplicationUser.NickName, ScoutNickName = events.ApplicationUser.ScoutNickName, EventId = events.EventId, Comment = events.Comment, CreatedUTC = events.CreatedUTC, ThumbPath = events.ThumbPath, IsEvent = events.IsEvent, IsPhoto = events.IsPhoto, IsVideo = events.IsVideo, RootID = events.RootID }).ToList();

            }
            foreach (ForumControll.PageContent res in dbResult)
            {
                string[] splittedRes = res.Comment.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                dctSearchTerms.ResetMatches();
                for (int i = 0; i < splittedRes.Length; i++)
                {
                    string curWordLower = splittedRes[i].ToLower();
                    foreach (string term in dctSearchTerms.Keys)
                    {
                        if (curWordLower.StartsWith(term))
                        {
                            dctSearchTerms[term] = true;
                            splittedRes[i] = string.Format(matchHighliht, curWordLower);
                            if (Properties.Settings.Default.FullWordSearch)
                            {
                                //full match zlepsi rating
                                if (curWordLower.TrimEnd(trimChars).Equals(term))
                                {
                                    res.SearchRating++;
                                }
                            }
                            break;
                        }
                    }
                }
                if (dctSearchTerms.All(s => s.Value == true))
                {
                    res.Comment = string.Join(" ", splittedRes);
                    commentResult.Add(res);
                }
            }
            if (Properties.Settings.Default.FullWordSearch)
            {
                commentResult = commentResult.OrderByDescending(r => r.SearchRating).ToList();
            }

            return commentResult;
        }


        private string generateFilesystemThumb(string localDir)
        {
            string[] res = localDir.Split(new char[] { Path.DirectorySeparatorChar });
            if (res == null)
                return string.Empty;
            switch (res.Length)
            {
                //rok
                case 1:
                    return string.Format("{0}/{1}/{2}", GlobalConstants.PthFileSystemRoot, res[0], GlobalConstants.fnRokFotka);                    
                //akcia
                case 2:
                    return string.Format("{0}/{1}/{2}/{3}", GlobalConstants.PthFileSystemRoot, res[0], res[1], GlobalConstants.fnAkciaFotka);                    
                //video
                case 4:
                    return  Path.Combine(GlobalConstants.PthFileSystemRoot, localDir, GlobalConstants.fnVideoPoster);                   
                default:
                    return string.Empty;
            }


        }

        private List<ForumControll.PageContent> SearchFileSystem()
        {
            #region adresar    

            Dictionary<string, ForumControll.PageContent> lstResult = new Dictionary<string, ForumControll.PageContent>();
                                
            string rootPath = Server.MapPath(GlobalConstants.PthFileSystemRoot);

            //toto bude chache 
            var directiories = Directory.EnumerateDirectories(rootPath, "*.*", searchOption: SearchOption.AllDirectories).Where(s => !s.ToLower().EndsWith(GlobalConstants.PthThumb.ToLower()));            
            List<string> directioriesMatch = new List<string>();
            int startIndex = rootPath.Length+1;            

            foreach (var dir in directiories)
            {
                dctSearchTerms.ResetMatches();
                string localDir = dir.Substring(startIndex, dir.Length - startIndex);
                string[] pathParts = localDir.ToLower().Split(new char[] {Path.DirectorySeparatorChar, splitters[0]},StringSplitOptions.RemoveEmptyEntries);
                if (pathParts[pathParts.Length - 1].Equals(GlobalConstants.fnVideoDir,StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                int rating = 0;
                foreach (string pathPart in pathParts)
                {
                    foreach (string term in dctSearchTerms.Keys)
                    {
                        if (pathPart.StartsWith(term))
                        {
                            dctSearchTerms[term] = true;
                            if (Properties.Settings.Default.FullWordSearch)
                            {
                                //full match zlepsi rating
                                if (pathPart.TrimEnd(trimChars).Equals(term))
                                {
                                    rating++;
                                }
                            }                            
                            break;
                        }
                    }
                }
                //ak sa nasli vsetky zhody podla search terms, tak cestu pridame do vysledkov.
                if (dctSearchTerms.All(s => s.Value == true))
                {                                      
                    lstResult.Add(dir, new ForumControll.PageContent() { EventId = localDir.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), ThumbPath= generateFilesystemThumb(localDir), SearchRating = rating, IsFileSystem = true });
                }
            }                                   
            #endregion adresar

            #region suborSkomentarom
            //ak sa najde uz rovnaky ako v adresary nad, tak sa len prida komentar a zlepsi sa rating
            var files = Directory.EnumerateFiles(Server.MapPath(GlobalConstants.PthFileSystemRoot),"*.txt", SearchOption.AllDirectories).Where(f=>f.EndsWith(GlobalConstants.fnAkciaPopis)|| f.EndsWith(GlobalConstants.fnRokPopis)|| f.EndsWith(GlobalConstants.fnVideoPopis));
            foreach (var file in files)
            {

                string fileContent = File.ReadAllText(file);
                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    continue;
                }
                dctSearchTerms.ResetMatches();
                int rating = 0;
                List<string> words = fileContent.Split(splitters)?.ToList();

                for (int i = 0; i < words.Count; i++)
                {
                    string curWordLower = words[i].ToLower();
                    foreach (string term in dctSearchTerms.Keys)
                    {
                        if (curWordLower.StartsWith(term))
                        {
                            dctSearchTerms[term] = true;
                            words[i] = string.Format(matchHighliht, curWordLower);
                            //full match zlepsi rating
                            if (Properties.Settings.Default.FullWordSearch)
                            {
                                if (curWordLower.TrimEnd(trimChars).Equals(term))
                                {
                                    rating++;
                                }
                            }
                            break;
                        }
                    }
                }
                if (dctSearchTerms.All(s => s.Value == true))
                {
                    string dir = Path.GetDirectoryName(file);
                    
                    
                    if (!lstResult.ContainsKey(dir))
                    {
                        string localDir = dir.Substring(startIndex, dir.Length - startIndex);
                        lstResult.Add(dir, new ForumControll.PageContent() {EventId = localDir.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),ThumbPath=  generateFilesystemThumb(localDir), Comment = string.Join(" ", words), SearchRating = rating, IsFileSystem = true });
                    }
                    else
                    { //ak uz sme nasli folder hore, tak len pridame do listu a zlepsime rating a pridame comment
                        ForumControll.PageContent result = (ForumControll.PageContent)lstResult[dir];
                        result.SearchRating += rating;
                        if (result.Comment == null)
                        {
                            result.Comment = string.Join(" ", words);
                        }
                    }
                }
            }
            #endregion suborSkomentarom

            List<ForumControll.PageContent> res = lstResult.Values.OrderByDescending(s=>s.SearchRating).ToList();
            return res;
        }
    }
}