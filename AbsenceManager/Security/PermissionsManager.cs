using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.DynamicData;

namespace AbsenceManager.Security
{
    public class PermissionsManager
    {
        public static bool IsInsertAvailable(MetaTable table)
        {
            if (UserIsAdmin()) return true;

            //foreach (MetaColumn column in table.Columns)
            //    if (!EditableColumn(column.DisplayName, table.Name)) return false;

            return false;
        }

        public static bool HasPageAcess(String tableName)
        {
            MembershipUser u = Membership.GetUser();
            string userName = u.UserName;

            AMRoleProvider gprp = (AMRoleProvider)Roles.Provider;
            ScreenPermissions sp = gprp.GetPermissionsForScreenInUser(tableName, userName);

            return sp != ScreenPermissions.None;
        }

        public static bool UserIsAdmin()
        {
            using (AM_Entities ent = new AM_Entities())
            {
                User u = GeneralHelpers.GetCurrentUser(ent);
                return u.IsAdmin;
            }
        }
    }
}