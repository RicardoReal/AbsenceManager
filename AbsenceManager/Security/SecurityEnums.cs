using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsenceManager.Security
{
    [Flags]
    public enum ScreenPermissions
    {
        None = 0,
        Read = 1,
        Write = 2
    }
}