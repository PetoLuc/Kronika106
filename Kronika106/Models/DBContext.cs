using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kronika106.Models
{
    /// <summary>
    /// DO model pre komentare
    /// </summary>
    public class Kronika106DBContext : IdentityDbContext<ApplicationUser>
    {
        public Kronika106DBContext() : base("DBKronika106")
        {            
        }    
        public static Kronika106DBContext Create()
        {
            return new Kronika106DBContext();
        }
        public DbSet<EventComments> Forum { get; set; }
        public DbSet<StatisticsSearch> StatisticsSearch { get; set; }
        public DbSet<StatisticBrowse> StatisticBrowse { get; set; }
    }
}
