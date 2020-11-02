using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kronika106.FileSystemModel
{
    public class Photo
    {
        public string Nazov { get; set; }
        public string Popis { get; set; }
        public string PathSliderPhoto { get; set; }
        public string PathThumbialPhoto { get; set; }
        public int PhotoIndex { get; set; }
    }
}