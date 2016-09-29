using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.DynamicData;
using System.Text;
using System.Collections.ObjectModel;

namespace AbsenceManager
{
    public static class FilterHistoryExtensionMethods
    {
        /// <summary>
        /// Adds the filters current value to the session.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        public static void AddFilterValueToSession(this Page page, MetaColumn column, Object value)
        {
            Dictionary<String, Object> filterValues;
            var objectName = column.Table.DataContextPropertyName;

            // get correct column name 
            var columnName = column.Name;
            if (column is MetaForeignKeyColumn)
                columnName = ((MetaForeignKeyColumn)column).ForeignKeyNames.ToCommaSeparatedString();

            // check to see if we already have a session object
            if (page.Session[objectName] != null)
                filterValues = (Dictionary<String, Object>)page.Session[objectName];
            else
                filterValues = new Dictionary<String, Object>();

            // add new filter value to session object
            if (filterValues.Keys.Contains(columnName))
                filterValues[columnName] = value;
            else
                filterValues.Add(columnName, value);

            // add back to session
            if (page.Session[objectName] != null)
                page.Session[objectName] = filterValues;
            else
                page.Session.Add(objectName, filterValues);
        }

        /// <summary>
        /// Gets the filter values from session.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="table">The table.</param>
        /// <param name="defaultValues">The default values.</param>
        /// <returns>An IDictionary of filter values from the session.</returns>
        public static IDictionary<String, Object> GetFilterValuesFromSession(this Page page, MetaTable table, IDictionary<String, Object> defaultValues)
        {
            var queryString = new StringBuilder();
            var objectName = table.DataContextPropertyName;
            if (page.Session[objectName] != null)
            {
                var sessionFilterValues = new Dictionary<String, Object>((Dictionary<String, Object>)page.Session[objectName]);

                foreach (string key in defaultValues.Keys)
                {
                    if (!sessionFilterValues.Keys.Contains(key) || sessionFilterValues[key] == null)
                        sessionFilterValues.Add(key, defaultValues[key]);
                    else
                        sessionFilterValues[key] = defaultValues[key];
                }
                var t = (Dictionary<String, Object>)page.Session[objectName];
                return sessionFilterValues;
            }
            else
                return defaultValues;
        }

        /// <summary>
        /// Clears the table filters.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="table">The table.</param>
        public static void ClearTableFilters(this Page page, MetaTable table)
        {
            var objectName = table.DataContextPropertyName;

            if (page.Session[objectName] != null)
                page.Session[objectName] = null;
        }

        /// <summary>
        /// Toes the comma separated string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static String ToCommaSeparatedString<T>(this ReadOnlyCollection<T> list)
        {
            var toString = new StringBuilder();
            foreach (var item in list)
            {
                toString.Append(item.ToString() + ",");
            }
            return toString.ToString().Substring(0, toString.Length - 1);
        }
    }
}