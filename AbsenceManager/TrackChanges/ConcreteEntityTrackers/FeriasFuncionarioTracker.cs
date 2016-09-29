using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.TrackChanges.ConcreteEntityTrackers
{
    public class FeriasFuncionarioTracker : IEntityTrackChanges
    {
        public void processChanges(TrackStruct entryStr, User u)
        {
            if (entryStr.entryState == System.Data.EntityState.Added)
                processAdded(entryStr.entry, u);
            else if (entryStr.entryState == System.Data.EntityState.Modified)
                processModified(entryStr.entry, u);
        }

        private void processAdded(ObjectStateEntry entry, User u)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                string machineToken = GeneralHelpers.GetMachineName();
                string[] addedFields = new string[] { "DataFerias", "NrHoras", "Observacoes", "UserID", "Aprovado", "Pendente" };
                string[] fields;
                FeriasFuncionario newFeriasFunc = (FeriasFuncionario)entry.Entity;

                foreach (String field in addedFields)
                {
                    fields = GetSingleFieldValue(ent, newFeriasFunc, field);

                    if (fields != null && !String.IsNullOrEmpty(fields[1]))
                    {
                        ent.TrackFeriasFuncionarios.AddObject
                            (
                                new TrackFeriasFuncionario(
                                    newFeriasFunc.ID,
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
                FeriasFuncionario newFeriasFunc = (FeriasFuncionario)entry.Entity;
                FeriasFuncionario oldFeriasFunc = (from f in ent.FeriasFuncionarios where f.ID == newFeriasFunc.ID select f).FirstOrDefault();
                string[] fields;

                IEnumerable<string> modifiedFields = entry.GetModifiedProperties();

                foreach (string mf in modifiedFields)
                {
                    fields = GetFieldValue(ent, newFeriasFunc, oldFeriasFunc, mf);
                    if (fields != null)
                    {
                        fieldName = fields[0];
                        newValue = fields[1];
                        oldValue = fields[2];

                        if (oldValue != newValue)
                        {
                            ent.TrackFeriasFuncionarios.AddObject
                            (
                                new TrackFeriasFuncionario(
                                    oldFeriasFunc.ID,
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

        private string[] GetFieldValue(AM_Entities ent, FeriasFuncionario newFeriasFunc, FeriasFuncionario oldFeriasFunc, string field)
        {
            switch (field)
            {
                case "UserID":
                    string newFunc = ent.Users.Where(x => x.ID == newFeriasFunc.UserID).FirstOrDefault().Nome;
                    string oldFunc = ent.Users.Where(x => x.ID == oldFeriasFunc.UserID).FirstOrDefault().Nome;
                    return new string[] { "Funcionário", newFunc, oldFunc };
                case "NrHoras":
                    return new string[] { "Nr. Horas", newFeriasFunc.NrHoras.ToString(), oldFeriasFunc.NrHoras.ToString() };
                case "DataFerias":
                    return new string[] { "Data", newFeriasFunc.DataFerias.ToShortDateString(), oldFeriasFunc.DataFerias.ToShortDateString() };
                case "Observacoes":
                    return new string[] { "Observações", newFeriasFunc.Observacoes, oldFeriasFunc.Observacoes };
                case "Aprovado":
                    return new string[] { "Aprovado", newFeriasFunc.Aprovado ? "Sim" : "Não", oldFeriasFunc.Aprovado ? "Sim" : "Não" };
                case "Pendente":
                    return new string[] { "Pendente", newFeriasFunc.Pendente ? "Sim" : "Não", oldFeriasFunc.Pendente ? "Sim" : "Não" };
                default:
                    return null;
            }
        }

        private string[] GetSingleFieldValue(AM_Entities ent, FeriasFuncionario newFeriasFunc, string field)
        {
            switch (field)
            {
                case "UserID":
                    string newFunc = ent.Users.Where(x => x.ID == newFeriasFunc.UserID).FirstOrDefault().Nome;
                    return new string[] { "Funcionário", newFunc };
                case "NrHoras":
                    return new string[] { "Nr. Horas", newFeriasFunc.NrHoras.ToString() };
                case "DataFerias":
                    return new string[] { "Data", newFeriasFunc.DataFerias.ToShortDateString() };
                case "Observacoes":
                    return new string[] { "Observações", newFeriasFunc.Observacoes };
                case "Aprovado":
                    return new string[] { "Aprovado", newFeriasFunc.Aprovado ? "Sim" : "Não" };
                case "Pendente":
                    return new string[] { "Pendente", newFeriasFunc.Pendente ? "Sim" : "Não" };

                default:
                    return null;
            }
        }
    }
}