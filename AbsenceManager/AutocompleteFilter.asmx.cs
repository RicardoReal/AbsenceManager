using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Runtime.CompilerServices;
using AbsenceManager.AutoComplete;
using AbsenceManager.AutoComplete.EntityAutoComplete;
using AjaxControlToolkit;
using System.Web.DynamicData;
using System.Diagnostics;
using System.Data.Objects.DataClasses;

namespace AbsenceManager
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutocompleteFilterService : System.Web.Services.WebService
    {
        private Dictionary<string, IAutoComplete> autocompletes;

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
            if (autocompletes == null) fillDictionary();

            MetaTable table = GetTable(contextKey);
            var values = new List<String>();

            if (autocompletes.ContainsKey(table.Name))
            {
                List<EntityObject> queryable = autocompletes[table.Name].getQuery(prefixText, count);

                foreach (var row in queryable)
                    values.Add(CreateAutoCompleteItem(table, row));
            }

            return values.Distinct().ToArray();
        }

        public static string GetContextKey(MetaTable parentTable)
        {
            return String.Format("{0}#{1}", parentTable.DataContextType.FullName, parentTable.Name);
        }

        public static MetaTable GetTable(string contextKey)
        {
            string[] param = contextKey.Split('#');
            Debug.Assert(param.Length == 2, String.Format("The context key '{0}' is invalid", contextKey));
            Type type = Type.GetType(param[0]);
            return MetaModel.GetModel(type).GetTable(param[1], type);
        }

        private static string CreateAutoCompleteItem(MetaTable table, object row)
        {
            return AutoCompleteExtender.CreateAutoCompleteItem(table.GetDisplayString(row), table.GetPrimaryKeyString(row));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void fillDictionary()
        {
            autocompletes = new Dictionary<string, IAutoComplete>();
            autocompletes.Add("Users", new FuncionarioAC());
        }
    }
}
