using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsenceManager.TrackChanges
{
    interface IEntityTrackChanges
    {
        void processChanges(TrackStruct entryStr, User u);
    }
}