using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;

namespace AbsenceManager.AutoComplete.EntityAutoComplete
{
    public class FuncionarioAC : IAutoComplete
    {
        public List<EntityObject> getQuery(string prefixText, int top)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                var q = ent.Users.Where(a => a.Nome.Contains(prefixText)).OrderBy(a => a.Nome).Take(top);

                if (q.Count() == 0)
                {
                    q = ent.Users.Where(a => a.NrFuncionario.Contains(prefixText)).OrderBy(a => a.NrFuncionario).Take(top);
                }

                return q.ToList<EntityObject>();
            }
        }
    }
}
