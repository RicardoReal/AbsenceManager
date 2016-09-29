using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsenceManager
{
    // Atributo usado para esconder um filtro
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HideFilterAttribute : Attribute
    {
        public bool Hide { get; private set; }

        public HideFilterAttribute()
        {
            Hide = false;
        }

        public HideFilterAttribute(bool hide)
        {
            Hide = hide;
        }
    }
}