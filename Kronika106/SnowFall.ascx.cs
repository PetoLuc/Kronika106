using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kronika106
{
    public partial class SnowFall : System.Web.UI.UserControl
    {
        private List<string> lstNoSnow = new List<string>() { "/Video" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lstNoSnow.Contains(Request.FilePath))
            {
                this.Controls.Clear();
            }
        }
    }
}