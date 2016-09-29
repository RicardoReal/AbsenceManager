using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using AbsenceManager.AppUtils;

namespace AbsenceManager.Security
{
    public class TableRegister
    {
        // Regista as tabelas do Model na ApplicationScreens
        public static void RegisterApplicationScreens()
        {
            try
            {
                using (AM_Entities ent = new AM_Entities())
                {

                    ArrayList tablesInModel = new ArrayList();
                    ArrayList tablesInDB = new ArrayList();

                    //String[] adminScreens = System.Configuration.ConfigurationManager.AppSettings["AdminScreens"].Split(',');
                    String[] specialScreens = System.Configuration.ConfigurationManager.AppSettings["SpecialScreens"].Split(',');

                    foreach (Object t in Global.DefaultModel.Tables)
                    {
                        //if (!adminScreens.Contains(((System.Web.DynamicData.MetaTable)t).Name))
                        tablesInModel.Add(((System.Web.DynamicData.MetaTable)t).Name);
                    }

                    foreach (String s in specialScreens)
                    {
                        tablesInModel.Add(s);
                    }

                    var appScreens = (from aps in ent.ApplicationScreens select aps);

                    try
                    {
                        foreach (ApplicationScreen appS in appScreens)
                            tablesInDB.Add(appS.ScreenName);

                        foreach (string tableInModel in tablesInModel)
                        {
                            if (!tablesInDB.Contains(tableInModel))
                            {
                                ApplicationScreen appS = new ApplicationScreen();
                                appS.ScreenName = tableInModel;
                                ent.ApplicationScreens.AddObject(appS);
                                ent.SaveChanges();
                            }
                        }

                        //foreach (string tableInDB in tablesInDB)
                        //{
                        //    if (!tablesInModel.Contains(tableInDB))
                        //    {
                        //        var aps = (from a in ent.ApplicationScreens
                        //                   where a.ScreenName == tableInDB
                        //                   select a).First();

                        //        var ras = (from r in ent.RoleApplicationScreens
                        //                   where r.ApplicationScreenID == aps.ID
                        //                   select r);

                        //        //foreach (RoleApplicationScreen roleAppScreen in ras)
                        //        //    ent.RoleApplicationScreens.DeleteObject(roleAppScreen);

                        //        ent.ApplicationScreens.DeleteObject(aps);
                        //        ent.SaveChanges();
                        //    }
                        //}
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "AM_TablesRegister");
            }
        }
    }
}