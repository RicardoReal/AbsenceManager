using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations;
using AbsenceManager.AppUtils;

namespace AbsenceManager.ModelLogic.EntitiesLogic
{
    public class FuncionarioHorasFuncionarioLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            try
            {
                FuncionarioHorasFuncionario hf = entry.Entity as FuncionarioHorasFuncionario;

                TipoHora thr = ent.TipoHoras.Where(x => x.Descricao == "Banco de Horas").FirstOrDefault();

                if (hf.NrHoras > 8) throw new ValidationException("O Nr. de horas introduzidas não pode ser superior a 8.");

                if (hf.TipoHorasID == thr.ID)
                {
                    using (AM_Entities _ent = new AM_Entities())
                    {
                        User u = _ent.Users.Where(x => x.ID == hf.UserID).FirstOrDefault();

                        HorasFuncionario horasfunc = ent.HorasFuncionarios.Where(x => x.ID == hf.HorasID).FirstOrDefault();

                        if (entry.State == System.Data.EntityState.Modified)
                        {
                            horasfunc.Ano = horasfunc.Data.Year;
                            horasfunc.Mes = horasfunc.Data.Month;
                            horasfunc.Dia = horasfunc.Data.Day;
                            u.Compensacoes -= (decimal)(entry.OriginalValues["NrHoras"]);
                            u.Compensacoes += hf.NrHoras;
                        }
                        else if (entry.State == System.Data.EntityState.Added)
                        {
                            horasfunc.Ano = horasfunc.Data.Year;
                            horasfunc.Mes = horasfunc.Data.Month;
                            horasfunc.Dia = horasfunc.Data.Day;
                            u.Compensacoes += hf.NrHoras;
                        }

                        else if (entry.State == System.Data.EntityState.Deleted)
                        {
                            FuncionarioHorasFuncionario fhf = _ent.FuncionarioHorasFuncionarios.Where(x => x.ID == hf.ID).FirstOrDefault();
                            u.Compensacoes -= fhf.NrHoras;
                        }


                        _ent.SaveChanges();

                    }

                }

                if (entry.State == System.Data.EntityState.Added)
                {
                    using (AM_Entities _ent = new AM_Entities())
                    {
                        HorasFuncionario h = _ent.HorasFuncionarios.Where(x => x.ID == hf.HorasID).FirstOrDefault();

                        int horasFunc = (from fhf in ent.FuncionarioHorasFuncionarios
                                         join th in ent.TipoHoras on fhf.TipoHorasID equals th.ID
                                         where fhf.HorasID == hf.HorasID
                                         && fhf.UserID == hf.UserID
                                         && th.HorasExtra == false
                                         select fhf).ToList().Count;

                        if (horasFunc == 0)
                        {
                            h.NrPresencas += 1;
                            
                        }

                        _ent.SaveChanges();
                    }
                }

                if (entry.State == System.Data.EntityState.Deleted)
                {
                    using (AM_Entities _ent = new AM_Entities())
                    {
                        int horasFunc = (from fhf in ent.FuncionarioHorasFuncionarios
                                         join th in ent.TipoHoras on fhf.TipoHorasID equals th.ID
                                         where fhf.HorasID == hf.HorasID
                                         && fhf.UserID == hf.UserID
                                         && th.HorasExtra == false
                                         select fhf).ToList().Count;

                        if (horasFunc == 1)
                        {
                            HorasFuncionario h = _ent.HorasFuncionarios.Where(x => x.ID == hf.HorasID).FirstOrDefault();
                            h.NrPresencas -= 1; // Se o func. for o mesmo, não pode contar 2x
                        }

                        _ent.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ValidationException)) throw ex;
                Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "FuncHorasFuncLogic");
            }
        }
    }
}