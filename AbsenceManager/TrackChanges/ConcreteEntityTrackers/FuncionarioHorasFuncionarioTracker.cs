using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.TrackChanges.ConcreteEntityTrackers
{
    public class FuncionarioHorasFuncionarioTracker : IEntityTrackChanges
    {
        public void processChanges(TrackStruct entryStr, User u)
        {
            if (entryStr.entryState == System.Data.EntityState.Added)
                processAdded(entryStr.entry, u);
            else if (entryStr.entryState == System.Data.EntityState.Modified)
                processModified(entryStr.entry, u);
            else if (entryStr.entryState == System.Data.EntityState.Deleted)
                processDeleted(entryStr.entry, u);
        }

        private void processAdded(ObjectStateEntry entry, User u)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                string machineToken = GeneralHelpers.GetMachineName();
                string[] addedFields = new string[] { "UserID", "NrHoras", "TipoHorasID" };
                string[] fields;
                FuncionarioHorasFuncionario newFuncHorasFunc = (FuncionarioHorasFuncionario)entry.Entity;
                

                foreach (String field in addedFields)
                {
                    fields = GetSingleFieldValue(ent, newFuncHorasFunc, field);

                    if (fields != null && !String.IsNullOrEmpty(fields[1]))
                    {
                        ent.TrackHorasFuncionarios.AddObject
                            (
                                new TrackHorasFuncionario(
                                    newFuncHorasFunc.HorasID,
                                    fields[0],
                                    "",
                                    fields[1],
                                    u.Nome,
                                    DateTime.Now,
                                    machineToken)
                             );
                    }
                }

                ent.SaveChanges();
            }
        }

        private void processModified(ObjectStateEntry entry, User u)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                string oldValue, newValue, fieldName;
                string machineToken = GeneralHelpers.GetMachineName();
                FuncionarioHorasFuncionario newFuncHorasFunc = (FuncionarioHorasFuncionario)entry.Entity;
                FuncionarioHorasFuncionario oldFuncHorasFunc = (from f in ent.FuncionarioHorasFuncionarios where f.ID == newFuncHorasFunc.ID select f).FirstOrDefault();
                string[] fields;

                IEnumerable<string> modifiedFields = entry.GetModifiedProperties();

                foreach (string mf in modifiedFields)
                {
                    fields = GetFieldValue(ent, newFuncHorasFunc, oldFuncHorasFunc, mf);
                    if (fields != null)
                    {
                        fieldName = fields[0];
                        newValue = fields[1];
                        oldValue = fields[2];

                        if (oldValue != newValue)
                        {
                            ent.TrackHorasFuncionarios.AddObject
                            (
                                new TrackHorasFuncionario(
                                    oldFuncHorasFunc.HorasID,
                                    fieldName,
                                    oldValue,
                                    newValue,
                                    u.Nome,
                                    DateTime.Now,
                                    machineToken)
                            );
                        }
                    }
                }
                ent.SaveChanges();
            }
        }

        private void processDeleted(ObjectStateEntry entry, User u)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                FuncionarioHorasFuncionario removedFuncHorasFunc = (FuncionarioHorasFuncionario)entry.Entity;

                string nomeFunc = (from user in ent.Users
                                   join fhf in ent.FuncionarioHorasFuncionarios on user.ID equals fhf.UserID
                                   where fhf.ID == removedFuncHorasFunc.ID
                                   select user.Nome).FirstOrDefault();

                string machineToken = GeneralHelpers.GetMachineName();

                ent.TrackHorasFuncionarios.AddObject
                            (
                                new TrackHorasFuncionario(
                                    removedFuncHorasFunc.ID,
                                    nomeFunc + " Removido",
                                    "",
                                    "",
                                    u.Nome,
                                    DateTime.Now,
                                    machineToken)
                             );

                ent.SaveChanges();
            }
        }

        private string[] GetFieldValue(AM_Entities ent, FuncionarioHorasFuncionario newFuncHorasFunc, FuncionarioHorasFuncionario oldFuncHorasFunc, string field)
        {
            string funcName = ent.Users.Where(x => x.ID == newFuncHorasFunc.UserID).FirstOrDefault().Nome;
            switch (field)
            {
                case "UserID":
                    string oldFuncName = ent.Users.Where(x => x.ID == oldFuncHorasFunc.UserID).FirstOrDefault().Nome;
                    return new string[] { "Func.", funcName, oldFuncName };
                case "NrHoras":
                    return new string[] { "Func." + funcName + " : Nr. Horas", newFuncHorasFunc.NrHoras.ToString(), oldFuncHorasFunc.NrHoras.ToString() };
                case "TipoHorasID":
                    string newTipoHoras = ent.TipoHoras.Where(x => x.ID == newFuncHorasFunc.TipoHorasID).FirstOrDefault().Descricao;
                    string oldTipoHoras = ent.TipoHoras.Where(x => x.ID == oldFuncHorasFunc.TipoHorasID).FirstOrDefault().Descricao;
                    return new string[] { "Func." + funcName + " : Tipo Horas", newTipoHoras, oldTipoHoras };
                
                default:
                    return null;
            }
        }

        private string[] GetSingleFieldValue(AM_Entities ent, FuncionarioHorasFuncionario newFuncHorasFunc, string field)
        {
            string funcName = ent.Users.Where(x => x.ID == newFuncHorasFunc.UserID).FirstOrDefault().Nome;
            switch (field)
            {
                case "UserID":
                    return new string[] { "Func.", funcName };
                case "NrHoras":
                    return new string[] { "Func." + funcName + " : Nr. Horas", newFuncHorasFunc.NrHoras.ToString() };
                case "TipoHorasID":
                    string newTipoHoras = ent.TipoHoras.Where(x => x.ID == newFuncHorasFunc.TipoHorasID).FirstOrDefault().Descricao;
                    return new string[] { "Func." + funcName + " : Tipo Horas", newTipoHoras };
                
                default:
                    return null;
            }
        }
    }
}