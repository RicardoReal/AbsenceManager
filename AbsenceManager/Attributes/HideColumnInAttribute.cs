using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsenceManager
{
    // Atributo usado para esconder uma coluna em um ou mais PageTemplates
    [AttributeUsage(AttributeTargets.Property)]
    public class HideColumnInAttribute : Attribute
    {
        public PageTemplate[] PageTemplates { get; private set; }

        public HideColumnInAttribute()
        {
            PageTemplates = new PageTemplate[0];
        }

        public HideColumnInAttribute(params PageTemplate[] lookupTable)
        {
            PageTemplates = lookupTable;
        }
    }
}