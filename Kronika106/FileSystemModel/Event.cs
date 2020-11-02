using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kronika106.FileSystemModel
{
    public class Akcia
    {
        public string Nazov { get; set; }
        public string Popis { get; set; }
        public string PathFotka { get; set; }
        public string URL { get; set; }
        public long PocetKomentarov { get; set; }

        public string PathAkcia { get; set; }  
    }
}