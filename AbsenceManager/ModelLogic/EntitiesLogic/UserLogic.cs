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
    public class UserLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            try
            {
                User u = entry.Entity as User;

                AMMembershipProvider gmp = (AMMembershipProvider)Membership.Provider;

                if (entry.GetModifiedProperties().Contains("Password") && !string.IsNullOrEmpty(u.Password))
                {
                    u.Password = gmp.EncodePassword(u.Password);
                }

                if (entry.GetModifiedProperties().Contains("EmpresaID") || entry.State == System.Data.EntityState.Added)
                {
                    Empresa emp = ent.Empresas.Where(e => e.ID == u.EmpresaID).FirstOrDefault();
                    u.CustoHora = emp.ValorPorFunc == null ? 0 : (decimal)emp.ValorPorFunc;
                    //u.Password = gmp.EncodePassword(u.Password);
                }

                if (u.ID == 0)
                {
                    if (!string.IsNullOrEmpty(u.Password)) u.Password = gmp.EncodePassword(u.Password);
                    u.CreationDate = DateTime.Now;

                    if (ent.Users.Where(us => us.NrFuncionario == u.NrFuncionario).Count() > 0)
                    {
                        throw new ValidationException("Número de funcionário já atribuido.");
                    }
                }
                else
                {
                    if (entry.GetModifiedProperties().Contains("NrFuncionario") && ent.Users.Where(us => us.NrFuncionario == u.NrFuncionario && us.ID != u.ID).Count() > 0)
                    {
                        throw new ValidationException("Número de funcionário já atribuido.");
                    }
                }

                if (entry.State == System.Data.EntityState.Added) u.Activo = true;
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ValidationException)) throw e;
                Utils.WriteErrorLog(e.Message, e.InnerException, e.StackTrace, "UserLogic");
            }
        }
    }
}