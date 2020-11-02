using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kronika106.Models
{
    public class StatisticsSearch: EntityBase
    {
        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string SearchPattern { get; set; }
    }
}