using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.TrackChanges.ConcreteEntityTrackers
{
    public class AusenciaFuncionarioTracker : IEntityTrackChanges
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
                string[] addedFields = new string[] { "DataAusencia", "Observacoes", "NrHoras", "TipoAusenciaID", "UserID" };
                string[] fields;
                AusenciaFuncionario newAusenciaFunc = (AusenciaFuncionario)entry.Entity;

                foreach (String field in addedFields)
                {
                    fields = GetSingleFieldValue(ent, newAusenciaFunc, field);

                    if (fields != null && !String.IsNullOrEmpty(fields[1]))
                    {
                        ent.TrackAusenciaFuncionarios.AddObject
                            (
                                new TrackAusenciaFuncionario(
                                    newAusenciaFunc.ID,
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
                AusenciaFuncionario newAusenciaFunc = (AusenciaFuncionario)entry.Entity;
                AusenciaFuncionario oldAusenciaFunc = (from f in ent.AusenciaFuncionarios where f.ID == newAusenciaFunc.ID select f).FirstOrDefault();
                string[] fields;

                IEnumerable<string> modifiedFields = entry.GetModifiedProperties();

                foreach (string mf in modifiedFields)
                {
                    fields = GetFieldValue(ent, newAusenciaFunc, oldAusenciaFunc, mf);
                    if (fields != null)
                    {
                        fieldName = fields[0];
                        newValue = fields[1];
                        oldValue = fields[2];

                        if (oldValue != newValue)
                        {
                            ent.TrackAusenciaFuncionarios.AddObject
                            (
                                new TrackAusenciaFuncionario(
                                    oldAusenciaFunc.ID,
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
                AusenciaFuncionario removedAusenciaFunc = (AusenciaFuncionario)entry.Entity;

                string machineToken = GeneralHelpers.GetMachineName();

                ent.TrackAusenciaFuncionarios.AddObject
                            (
                                new TrackAusenciaFuncionario(
                                    removedAusenciaFunc.ID,
                                    "REMOVIDO",
                                    "",
                                    "",
                                    u.Nome,
                                    DateTime.Now,
                                    machineToken)
                             );

                ent.SaveChanges();
            }
        }

        private string[] GetFieldValue(AM_Entities ent, AusenciaFuncionario newAusenciaFunc, AusenciaFuncionario oldAusenciaFunc, string field)
        {
            switch (field)
            {
                case "UserID":
                    string newFunc = ent.Users.Where(x => x.ID == newAusenciaFunc.UserID).FirstOrDefault().Nome;
                    string oldFunc = ent.Users.Where(x => x.ID == oldAusenciaFunc.UserID).FirstOrDefault().Nome;
                    return new string[] { "Funcionário", newFunc, oldFunc };
                case "DataAusencia":
                    return new string[] { "Data", newAusenciaFunc.DataAusencia.ToShortDateString(), oldAusenciaFunc.DataAusencia.ToShortDateString() };
                case "TipoAusenciaID":
                    string newTipoAus = ent.TipoAusencias.Where(x => x.ID == newAusenciaFunc.TipoAusenciaID).FirstOrDefault().Descricao;
                    string oldTipoAus = ent.TipoAusencias.Where(x => x.ID == oldAusenciaFunc.TipoAusenciaID).FirstOrDefault().Descricao;
                    return new string[] { "Tipo de Ausência", newTipoAus, oldTipoAus };
                case "NrHoras":
                    return new string[] { "Nr. Horas", newAusenciaFunc.NrHoras.ToString(), oldAusenciaFunc.NrHoras.ToString() };
                case "Observacoes":
                    return new string[] { "Observações", newAusenciaFunc.Observacoes, oldAusenciaFunc.Observacoes };

                default:
                    return null;
            }
        }

        private string[] GetSingleFieldValue(AM_Entities ent, AusenciaFuncionario newAusenciaFunc, string field)
        {
            switch (field)
            {
                case "UserID":
                    string newFunc = ent.Users.Where(x => x.ID == newAusenciaFunc.UserID).FirstOrDefault().Nome;
                    return new string[] { "Funcionário", newFunc };
                case "DataAusencia":
                    return new string[] { "Data", newAusenciaFunc.DataAusencia.ToShortDateString() };
                case "TipoAusenciaID":
                    string newTipoAus = ent.TipoAusencias.Where(x => x.ID == newAusenciaFunc.TipoAusenciaID).FirstOrDefault().Descricao;
                    return new string[] { "Tipo de Ausência", newTipoAus };
                case "NrHoras":
                    return new string[] { "Nr. Horas", newAusenciaFunc.NrHoras.ToString() };
                case "Observacoes":
                    return new string[] { "Observações", newAusenciaFunc.Observacoes };

                default:
                    return null;
            }
        }
    }
}