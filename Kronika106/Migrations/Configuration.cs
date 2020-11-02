namespace Kronika106.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Kronika106.Models.Kronika106DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Kronika106.Models.Kronika106DBContext";
        }

        protected override void Seed(Kronika106.Models.Kronika106DBContext context)
        {       
        }
    }
}
