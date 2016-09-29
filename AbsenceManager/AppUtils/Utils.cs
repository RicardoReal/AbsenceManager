using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbsenceManager.AppUtils
{
    public class Utils
    {
        public static void WriteErrorLog(string errorMessage, Exception innerMsg, string stackTrace, string Module)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                ErrorLog _errorLog = new ErrorLog();

                _errorLog.Message = errorMessage;
                if (innerMsg != null) _errorLog.InnerMessage = innerMsg.ToString();
                _errorLog.StackTrace = stackTrace;
                _errorLog.Module = Module;
                _errorLog.ErrorDateTime = DateTime.Now;

                ent.ErrorLogs.AddObject(_errorLog);
                try
                {
                    ent.SaveChanges();
                }
                catch { }
            }
        }
    }
}