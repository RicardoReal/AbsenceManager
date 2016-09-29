using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsenceManager.AutoComplete
{
    interface IAutoComplete
    {
        List<System.Data.Objects.DataClasses.EntityObject> getQuery(string prefixText, int top);
    }
}