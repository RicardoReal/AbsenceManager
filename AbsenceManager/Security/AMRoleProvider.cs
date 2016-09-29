using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using System.Configuration.Provider;
using AbsenceManager.Security;

namespace AbsenceManager
{
    public class AMRoleProvider : RoleProvider
    {
        

        public override string ApplicationName
        {
            get
            {
                return "Absence Manager";
            }
            set { }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "AMRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "AbsenceManager Role provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);
        }

        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                if (roleName.Contains(",")) throw new ArgumentException("Role names cannot contain commas.");
                if (RoleExists(roleName)) throw new ProviderException("Role name already exists.");

                Role r = new Role();
                r.Role1 = roleName;
                ent.Roles.AddObject(r);
                ent.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                if (!RoleExists(roleName)) throw new ProviderException("Role does not exist.");
                if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0) throw new ProviderException("Cannot delete a populated role.");

                Role role = (from r in ent.Roles where r.Role1 == roleName select r).First();
                ent.Roles.DeleteObject(role);

                ent.SaveChanges();
            }
            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                return (from u in ent.Users
                        join r in ent.Roles on u.RoleID equals r.ID
                        where r.Role1 == roleName && u.UserName.Contains(usernameToMatch)
                        select u.UserName).ToArray();
            }
        }

        public override string[] GetAllRoles()
        {
            using (AM_Entities ent = new AM_Entities())
            {
                return (from r in ent.Roles select r.Role1).ToArray();
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                return (from u in ent.Users
                        join r in ent.Roles on u.RoleID equals r.ID
                        where r.Role1 == roleName
                        select u.UserName).ToArray();
            }
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                var ct = (from u in ent.Users
                          join r in ent.Roles on u.RoleID equals r.ID
                          where r.Role1.Contains(roleName) && u.UserName == userName
                          select u).FirstOrDefault();

                return ct != null;
            }
        }

        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                return (from r in ent.Roles where r.Role1 == roleName select r).First() != null;
            }
        }

        public ScreenPermissions GetPermissionsForScreenInUser(string screenName, string userName)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                bool userIsAdmin = (from u in ent.Users
                                    where u.UserName == userName
                                    select u.IsAdmin).FirstOrDefault();

                if (userIsAdmin) return ScreenPermissions.Write;

                var permissions = (from u in ent.Users
                                   join r in ent.Roles on u.RoleID equals r.ID
                                   join ras in ent.RoleApplicationScreens on r.ID equals ras.RoleID
                                   join aps in ent.ApplicationScreens on ras.ApplicationScreenID equals aps.ID
                                   where u.UserName == userName && aps.ScreenName == screenName
                                   select ras).ToList();

                ScreenPermissions sp = ScreenPermissions.None;

                foreach (RoleApplicationScreen permission in permissions)
                {
                    if (permission.WritePermission)
                        return ScreenPermissions.Write;
                    else if (permission.ReadPermission)
                        sp = ScreenPermissions.Read;
                }

                return sp;
            }
        }

        public bool UserCanTrackChanges(string screenName, string userName)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                bool userIsAdmin = (from u in ent.Users
                                    where u.UserName == userName
                                    select u.IsAdmin).FirstOrDefault();

                if (userIsAdmin) return true;

                var permissions = (from u in ent.Users
                                   join r in ent.Roles on u.RoleID equals r.ID
                                   join ras in ent.RoleApplicationScreens on r.ID equals ras.RoleID
                                   join aps in ent.ApplicationScreens on ras.ApplicationScreenID equals aps.ID
                                   where u.UserName == userName && aps.ScreenName == screenName
                                   select ras);

                if (permissions.Where(p => p.CanTrackChanges).Count() > 0) return true;

                return false;
            }
        }
    }
}