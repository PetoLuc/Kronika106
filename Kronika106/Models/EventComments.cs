using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kronika106.Models
{

	//zmazat migrations adtesar
//	Enable-migrations
//Add-migration “initial”
//update-database
	public class EventComments :EntityBase
    {
        //[Key, Required, ScaffoldColumn(false)]
        //public string Rok { get; set; }

        //[Key, Required, ScaffoldColumn(false)]
        //public string Akcia { get; set; }        

        [Required, StringLength(150)]
        public string EventId { get; set; }        

        [Required, StringLength(5000), DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public string ThumbPath { get; set; }

        [Required]
        public bool IsEvent { get; set; }

        [Required]
        public bool IsPhoto { get; set; }

        [Required]
        public bool IsVideo { get; set; }

        [Required]
        public virtual  ApplicationUser ApplicationUser { get; set; }
        
        public int? RootID { get; set; }

		//public TimeSpan? VideoPosition { get; set; }
	}
}