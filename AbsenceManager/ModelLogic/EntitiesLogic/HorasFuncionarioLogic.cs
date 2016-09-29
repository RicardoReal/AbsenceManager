using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations;
using AbsenceManager.AppUtils;

namespace AbsenceManager.ModelLogic.EntitiesLogic
{
    public class HorasFuncionarioLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            try
            {
                if (entry.State == System.Data.EntityState.Added)
                {
                    HorasFuncionario hf = entry.Entity as HorasFuncionario;

                    var horasDia = ent.HorasFuncionarios.Where(x => x.Data == hf.Data && x.DepartamentoID == hf.DepartamentoID).FirstOrDefault();

                    if(horasDia != null)
                        throw new ValidationException("O dia e departamento indicado já tem um registo de horas criado.");
                }
                else if (entry.State == System.Data.EntityState.Deleted)
                {
                    HorasFuncionario hf = entry.Entity as HorasFuncionario;
                    var funcs = (from horas in ent.FuncionarioHorasFuncionarios
                                 where horas.HorasID == hf.ID
                                 select horas).ToList().Count;

                    if (funcs > 0)
                    {
                        throw new ValidationException("Não é possivel apagar o dia seleccionado, pois existem horas de funcionários preenchidas.");
                    }
                }
            }
            catch (Exception e)
            {
                Utils.WriteErrorLog(e.Message, e.InnerException, e.StackTrace, "HorasFuncLogic");
                if (e.GetType() == typeof(ValidationException)) throw e;
            }


        }
    }
}