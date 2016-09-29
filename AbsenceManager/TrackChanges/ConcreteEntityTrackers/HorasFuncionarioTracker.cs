using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.TrackChanges.ConcreteEntityTrackers
{
    public class HorasFuncionarioTracker : IEntityTrackChanges
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
                string[] addedFields = new string[] { "Data", "Observacoes" };
                string[] fields;
                HorasFuncionario newHorasFunc = (HorasFuncionario)entry.Entity;

                foreach (String field in addedFields)
                {
                    fields = GetSingleFieldValue(ent, newHorasFunc, field);

                    if (fields != null && !String.IsNullOrEmpty(fields[1]))
                    {
                        ent.TrackHorasFuncionarios.AddObject
                            (
                                new TrackHorasFuncionario(
                                    newHorasFunc.ID,
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
                HorasFuncionario newHorasFunc = (HorasFuncionario)entry.Entity;
                HorasFuncionario oldHorasFunc = (from f in ent.HorasFuncionarios where f.ID == newHorasFunc.ID select f).FirstOrDefault();
                string[] fields;

                IEnumerable<string> modifiedFields = entry.GetModifiedProperties();

                foreach (string mf in modifiedFields)
                {
                    fields = GetFieldValue(ent, newHorasFunc, oldHorasFunc, mf);
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
                                    oldHorasFunc.ID,
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

        private string[] GetFieldValue(AM_Entities ent, HorasFuncionario newHorasFunc, HorasFuncionario oldHorasFunc, string field)
        {
            switch (field)
            {
                case "Data":
                    return new string[] { "Data", newHorasFunc.Data.ToShortDateString(), oldHorasFunc.Data.ToShortDateString() };
                case "Observacoes":
                    return new string[] { "Observações", newHorasFunc.Observacoes, oldHorasFunc.Observacoes };
                
                default:
                    return null;
            }
        }

        private string[] GetSingleFieldValue(AM_Entities ent, HorasFuncionario newHorasFunc, string field)
        {
            switch (field)
            {
                case "Data":
                    return new string[] { "Data", newHorasFunc.Data.ToShortDateString() };
                case "Observacoes":
                    return new string[] { "Observações", newHorasFunc.Observacoes };

                default:
                    return null;
            }
        }
    }
}