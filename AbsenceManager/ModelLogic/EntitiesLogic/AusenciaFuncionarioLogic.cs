using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.ModelLogic.EntitiesLogic
{
    public class AusenciaFuncionarioLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            

            AusenciaFuncionario f = entry.Entity as AusenciaFuncionario;

            

            TipoAusencia bh = ent.TipoAusencias.Where(x => x.Descricao == "Banco de Horas").FirstOrDefault();

            if(f.TipoAusenciaID == bh.ID)
            {
                using (AM_Entities _ent = new AM_Entities())
                {
                    User u = _ent.Users.Where(x => x.ID == f.UserID).FirstOrDefault();

                    if (entry.State == System.Data.EntityState.Modified)
                    {
                        f.Ano = f.DataAusencia.Year;
                        f.Mes = f.DataAusencia.Month;
                        f.Dia = f.DataAusencia.Day;
                        u.Compensacoes += (int)(entry.OriginalValues["NrHoras"]);
                        u.Compensacoes -= f.NrHoras;
                    }
                    else if(entry.State == System.Data.EntityState.Added)
                    {
                        f.Ano = f.DataAusencia.Year;
                        f.Mes = f.DataAusencia.Month;
                        f.Dia = f.DataAusencia.Day;
                        u.Compensacoes -= f.NrHoras;
                    }
                        
                    else if (entry.State == System.Data.EntityState.Deleted)
                    {
                        AusenciaFuncionario af = _ent.AusenciaFuncionarios.Where(x => x.ID == f.ID).FirstOrDefault();
                        u.Compensacoes += af.NrHoras;
                    }
                        

                    _ent.SaveChanges();

                }
                    
            }
        }
    }
}