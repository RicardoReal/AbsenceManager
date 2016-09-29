using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using AbsenceManager.AppUtils;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace AbsenceManager.ModelLogic.EntitiesLogic
{
    public class DepartamentoLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            try
            {
                if (entry.State == System.Data.EntityState.Deleted)
                {
                    Departamento dept = entry.Entity as Departamento;
                    if (ent.Users.Where(u => u.DepartamentoID == dept.ID).Count() > 0)
                    {
                        throw new ValidationException("Não é possivel apagar o dept. seleccionado, pois existem funcionários associados.");
                    }
                }

            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ValidationException)) throw e;
                Utils.WriteErrorLog(e.Message, e.InnerException, e.StackTrace, "DepartamentoLogic");
            }
        }
    }
}