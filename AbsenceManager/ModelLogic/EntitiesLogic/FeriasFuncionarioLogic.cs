using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations;

namespace AbsenceManager.ModelLogic.EntitiesLogic
{
    public class FeriasFuncionarioLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            try
            {
                FeriasFuncionario f = entry.Entity as FeriasFuncionario;

                f.Ano = f.DataFerias.Year;
                f.Mes = f.DataFerias.Month;
                f.Dia = f.DataFerias.Day;

                if (entry.State == System.Data.EntityState.Added || entry.State == System.Data.EntityState.Modified)
                {
                    if (f.Aprovado && ent.FeriasFuncionarios.Where(ff => ff.UserID == f.UserID && ff.DataFerias == f.DataFerias && ff.ID != f.ID).Count() > 0)
                    {
                        throw new ValidationException("Férias já marcadas para este funcionário, no dia seleccionado.");
                    }
                }
            }
            catch (Exception e)
            {
                if (typeof(ValidationException) == e.GetType()) throw e;
                AppUtils.Utils.WriteErrorLog("TESTE FERIAS", e.InnerException, "", "FeriasFuncionarioLogic");
            }
        }
    }
}