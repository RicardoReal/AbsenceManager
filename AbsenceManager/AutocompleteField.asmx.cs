using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.Services;
using AjaxControlToolkit;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using AbsenceManager.AutoComplete;
using System.Data.Objects.DataClasses;
using System.Runtime.CompilerServices;
using AbsenceManager.AutoComplete.EntityAutoComplete;

namespace AbsenceManager
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutocompleteFieldService : System.Web.Services.WebService
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

        private static IQueryable BuildFilterQuery(MetaTable table, string prefixText, int maxCount, AM_Entities ent)
        {
            // Alterado de LINQ para EntitySQL


            string eSQLQuery = @"select value ac 
                                 from " + table.Name + @" as ac
                                 where ac." + table.DisplayColumn.Name + @" like '%" + prefixText + @"%'";

            eSQLQuery += @"order by ac." + table.DisplayColumn.Name + @" limit " + maxCount.ToString();


            IQueryable iq = ent.CreateQuery<object>(eSQLQuery).AsQueryable();

            return iq;
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