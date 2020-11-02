using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Kronika106.Models
{

	//public sealed class MigrationConfiguration : DbMigrationsConfiguration<Kronika106DBContext>
	//{
	//	public MigrationConfiguration()
	//	{
	//		AutomaticMigrationsEnabled = true;
			
	//	}

	//	//protected override void Seed(Kronika106DBContext context)
	//	//{
	//	//	//  This method will be called after migrating to the latest version.

	//	//	//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
	//	//	//  to avoid creating duplicate seed data. E.g.
	//	//	//
	//	//	//    context.People.AddOrUpdate(
	//	//	//      p => p.FullName,
	//	//	//      new Person { FullName = "Andrew Peters" },
	//	//	//      new Person { FullName = "Brice Lambson" },
	//	//	//      new Person { FullName = "Rowan Miller" }
	//	//	//    );
	//	//	//
	//	//}
	//}

	public class Kronika106DatabaseInitializer : CreateDatabaseIfNotExists<Kronika106DBContext>
    {        
    }
}