using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNet451_WebForm
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string url = "http://user/api/user";
                string result = DiscoveryHelper.discoveryClient.DoGet(url);
                Response.Write(result);
            }
            
        }
    }
}