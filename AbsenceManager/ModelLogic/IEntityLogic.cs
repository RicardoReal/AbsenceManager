using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.ModelLogic
{
    interface IEntityLogic
    {
        void processEntity(ObjectStateEntry entry, AM_Entities ent);
    }
}