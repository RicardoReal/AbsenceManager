using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data;
using System.ComponentModel.DataAnnotations;
using AbsenceManager.ModelLogic;
using AbsenceManager.TrackChanges;

namespace AbsenceManager
{
    public partial class AM_Entities
    {
        partial void OnContextCreated()
        {
            this.SavingChanges += new EventHandler(context_SavingChanges);
        }

        public override int SaveChanges(SaveOptions options)
        {
            int i = 0;
            Dictionary<ObjectStateEntry, EntityState> entitiesStructs = new Dictionary<ObjectStateEntry, EntityState>();

            try
            {
                foreach (var entity in this.ObjectStateManager.GetObjectStateEntries(EntityState.Added))
                    entitiesStructs.Add(entity, entity.State);

                i = base.SaveChanges(options);

                // TrackChanges
                foreach (var obj in entitiesStructs)
                    TrackChangesManager.updateTrackChanges(new TrackStruct(obj.Key, obj.Value), this);
            }
            catch (OptimisticConcurrencyException ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "AMEntities");
                base.Refresh(RefreshMode.ClientWins, this.ObjectStateManager.GetObjectStateEntries(EntityState.Modified));
                base.SaveChanges(options);
            }
            catch (Exception ex)
            {
                AppUtils.Utils.WriteErrorLog(ex.Message, ex.InnerException, ex.StackTrace, "AMEntities");
                if (ex.GetType() == typeof(ValidationException) || ex.GetType() == typeof(UpdateException)) throw ex;
            }
            return i;
        }

        private static void context_SavingChanges(object sender, EventArgs e)
        {
            AM_Entities _ent = sender as AM_Entities;

            foreach (ObjectStateEntry entry in (sender as ObjectContext).ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified))
            {
                if (entry.State == EntityState.Modified)
                {
                    TrackChangesManager.updateTrackChanges(new TrackStruct(entry, EntityState.Modified), _ent);
                }

                LogicManager.processEntity(entry, _ent);
            }

            // DELETED ENTITIES
            foreach (ObjectStateEntry entry in (sender as ObjectContext).ObjectStateManager.GetObjectStateEntries(EntityState.Deleted))
            {
                TrackChangesManager.updateTrackChanges(new TrackStruct(entry, EntityState.Deleted), _ent);

                LogicManager.processDeletedEntity(entry, _ent);
            }
        }
    }
}