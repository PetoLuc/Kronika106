using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Kronika106.Logic;
using Kronika106.Migrations;
using Kronika106.Models;
using System.IO;

namespace Kronika106
{
    public class Global : HttpApplication
    {

        FileSystemWatcher watcher = new FileSystemWatcher();
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            
            //Database.SetInitializer(new CreateDatabaseIfNotExists<Kronika106DBContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Kronika106DBContext, Configuration>());
            new RoleActions().AddUsersToAdminRole();
            
            

            
            //Priprava na cache
            //watcher.Path = Server.MapPath(GlobalConstants.PthFileSystemRoot);
            //watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            //watcher.Changed += Watcher_Changed;
            //watcher.Renamed += Watcher_Changed;
            //watcher.Deleted += Watcher_Changed;
            //watcher.Created += Watcher_Changed;
            //watcher.IncludeSubdirectories = true;            

            //watcher.EnableRaisingEvents = true;
            ////Application.Add("watcher", watcher );

        }

        //private void Watcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            if (exc is HttpUnhandledException)
            {
                // Pass the error on to the error page.
                Server.Transfer("Error.aspx?handler=Application_Error%20-%20Global.asax", true);
            }
        }
    }
}