using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data;

namespace AbsenceManager.TrackChanges
{
    public class TrackStruct
    {
        public ObjectStateEntry entry;
        public EntityState entryState;

        public TrackStruct(ObjectStateEntry e, EntityState es)
        {
            entry = e; entryState = es;
        }
    }
}