using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kronika106.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kronika106.Logic
{
	internal class RoleActions
	{		
		internal void AddUsersToAdminRole()
		{
			
			// Access the application context and create result variables.
			Models.Kronika106DBContext context = new Kronika106DBContext();
			IdentityResult IdRoleResult;
			IdentityResult IdUserResult;

			// Create a RoleStore object by using the ApplicationDbContext object. 
			// The RoleStore is only allowed to contain IdentityRole objects.
			var roleStore = new RoleStore<IdentityRole>(context);

			// Create a RoleManager object that is only allowed to contain IdentityRole objects.
			// When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
			var roleMgr = new RoleManager<IdentityRole>(roleStore);

			// Then, you create the "canEdit" role if it doesn't already exist.
			if (!roleMgr.RoleExists(GlobalConstants.RoleAdmin))
			{
				IdRoleResult = roleMgr.Create(new IdentityRole { Name = GlobalConstants.RoleAdmin });				
			}

			// Create a UserManager object based on the UserStore object and the ApplicationDbContext  
			// object. Note that you can create new objects and use them as parameters in
			// a single line of code, rather than using multiple lines of code, as you did
			// for the RoleManager object.
			var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

			if (!string.IsNullOrEmpty(Properties.Settings.Default.AdminEmails))
			{
				string[] adminMails = Properties.Settings.Default.AdminEmails.Split(';');
				if (adminMails.Length > 0)
				{
					for (int i = 0; i < adminMails.Length; i++)
					{

						var user = userMgr.FindByEmail(adminMails[i]);
						if (user != null)
						{
							if (!userMgr.IsInRole(user.Id, GlobalConstants.RoleAdmin))
							{																								
								IdUserResult = userMgr.AddToRole(user.Id, GlobalConstants.RoleAdmin);
							}
						}
					}
					context.SaveChanges();
				}
			}
		}
	}
}