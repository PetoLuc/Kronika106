using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kronika106.FileSystemModel
{    
    public class Video
    {
        public string Nazov { get; set; }
        public string Popis {get; set;}
        public string PathVideoFotka { get; set; }
        public string PathVideo { get; set; }
		public string MP4 { get; set; }
		public string OGV { get; set; }
		public string WEBM { get; set; }
		public string Poster { get; set; }
		public string DetailURL { get; set; }
		
	}
}