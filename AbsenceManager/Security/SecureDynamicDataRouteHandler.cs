using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.DynamicData;

namespace AbsenceManager.Security
{
    public class SecureDynamicDataRouteHandler : DynamicDataRouteHandler
    {
        public SecureDynamicDataRouteHandler() { }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="table">The table.</param>
        /// <param name="action">The action.</param>
        /// <returns>An IHttpHandler</returns>
        public override IHttpHandler CreateHandler(
            DynamicDataRoute route,
            MetaTable table,
            string action)
        {
            string userName = Membership.GetUser().UserName;

            AMRoleProvider gprp = (AMRoleProvider)Roles.Provider;
            ScreenPermissions sp = gprp.GetPermissionsForScreenInUser(table.Name, userName);

            switch (sp)
            {
                case ScreenPermissions.None:
                    return new NoPermissionHttpHandler();
                case ScreenPermissions.Read:
                    if (action == "Edit" || action == "Insert")
                        return null;
                    break;
                case ScreenPermissions.Write:
                    break;
                default:
                    return new NoPermissionHttpHandler();
            }

            return base.CreateHandler(route, table, action);
        }
    }
}