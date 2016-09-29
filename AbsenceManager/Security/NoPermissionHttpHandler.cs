using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsenceManager.Security
{
    public class NoPermissionHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Redirect("~/NoPermission.aspx");
        }
    }
}