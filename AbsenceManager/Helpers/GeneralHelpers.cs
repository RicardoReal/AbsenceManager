using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Security.Principal;

namespace AbsenceManager
{
    public class GeneralHelpers
    {
        public static string GetMachineName()
        {
            return HttpContext.Current == null ? "localhost" : HttpContext.Current.Request.UserHostName;
        }

        public static User GetCurrentUser(AM_Entities ent)
        {
            IPrincipal mu = HttpContext.Current == null ? null : HttpContext.Current.User;

            if (mu == null)
            {
                //if (Thread.CurrentPrincipal.Identity.Name.Contains("Timer") || Thread.CurrentPrincipal.Identity.Name.Contains("GOpads"))
                //    return (from a in ent.Users where a.Username == "GOPADS" select a).FirstOrDefault();

                //return (from a in ent.Users
                //        where a.Username == "GO"
                //        select a).FirstOrDefault();
                return null;
            }
            else
            {
                //// InterfaceCute é Temporário . Remover quando não necessário
                //if (Thread.CurrentPrincipal.Identity.Name == "InterfaceFBLB" || Thread.CurrentPrincipal.Identity.Name == "InterfaceCute")
                //    return (from a in ent.Users where a.Username == "InterfaceFBLB" select a).FirstOrDefault();


                if (!String.IsNullOrEmpty(mu.Identity.Name))
                {
                    return (from a in ent.Users
                            where a.UserName == mu.Identity.Name
                            select a).FirstOrDefault();
                }
                return null;
                //return (from a in ent.Users
                //        where a.Username == "GO"
                //        select a).FirstOrDefault();
            }
        }

        public static void ShowJavascriptAlert(string message)
        {
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            Page page = HttpContext.Current.CurrentHandler as Page;

            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterStartupScript(typeof(GeneralHelpers), "alert", script);
            }
        }

        //public static void ShowTrackChanges(Page page, long id)
        //{
        //    page.Response.Redirect("~/TrackFlights/List.aspx?FlightID=" + id.ToString(), false);
        //}

        public static void ShowLoginsLog(Page page, long id)
        {
            page.Response.Redirect("~/TrackLogins/List.aspx?UserID=" + id.ToString(), false);
        }

        public static string GetExceptionMessage(Exception ex)
        {
            if (ex.InnerException != null)
                return string.Format("Message:\n{0}\n\n-----------\n\nInnerMessage:\n{1}\n\n-----------\n\nStackTrace:\n{2}", ex.Message, ex.InnerException.Message, ex.StackTrace);
            else
                return string.Format("Message:\n{0}\n\n-----------\n\nStackTrace:\n{1}", ex.Message, ex.StackTrace);
        }
    }
}