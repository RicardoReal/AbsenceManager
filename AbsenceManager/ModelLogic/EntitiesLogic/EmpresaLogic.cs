using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using AbsenceManager.AppUtils;
using System.Web.Security;

namespace AbsenceManager.ModelLogic.EntitiesLogic
{
    public class EmpresaLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            try
            {
                using (AM_Entities _ent = new AM_Entities())
                {
                    Empresa emp = entry.Entity as Empresa;

                    if (entry.GetModifiedProperties().Contains("ValorPorFunc"))
                    {
                        List<User> funcs = _ent.Users.Where(u => u.EmpresaID == emp.ID).ToList();

                        foreach (User func in funcs)
                            func.CustoHora = emp.ValorPorFunc == null ? 0 : (decimal)emp.ValorPorFunc;

                        _ent.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
                Utils.WriteErrorLog(e.Message, e.InnerException, e.StackTrace, "UserLogic");
            }
        }
    }
}