using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace AbsenceManager
{
    [Flags]
    public enum PageTemplate
    {
        // standard page templates
        Details = 0x01,
        Edit = 0x02,
        Insert = 0x04,
        List = 0x08,
        ListDetails = 0x10,
        // custom page templates
        Unknown = 0xff,
    }

    public static class ControlExtensionsMethods
    {
        private const String extension = ".aspx";

        /// <summary>
        /// Gets the page template from the page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static PageTemplate GetPageTemplate(this Page page)
        {
            try
            {
                return (PageTemplate)Enum.Parse(typeof(PageTemplate),
                    page.RouteData.Values["action"].ToString());
            }
            catch (ArgumentException)
            {
                return PageTemplate.Unknown;
            }
        }
    }
}