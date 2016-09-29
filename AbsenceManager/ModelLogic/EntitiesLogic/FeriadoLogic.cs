using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace AbsenceManager.ModelLogic.EntitiesLogic
{
    public class FeriadoLogic : IEntityLogic
    {
        public void processEntity(ObjectStateEntry entry, AM_Entities ent)
        {
            Feriado f = entry.Entity as Feriado;

            f.Dia = f.FeriadoData.Day;
            f.Mes = f.FeriadoData.Month;
        }
    }
}