using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Diagnostics;
using System.Globalization;

namespace AbsenceManager.DynamicData.Filters.Helpers
{
    public enum QueryType
    {
        Contains,
        StartsWith,
        EndsWith
    }

    public static class LinqExpressionHelper
    {
        public static MethodCallExpression BuildSingleItemQuery(IQueryable query, MetaTable metaTable, string[] primaryKeyValues)
        {
            // Items.Where(row => row.ID == 1)
            var whereCall = BuildItemsQuery(query, metaTable, metaTable.PrimaryKeyColumns, primaryKeyValues);
            // Items.Where(row => row.ID == 1).Single()
            var singleCall = Expression.Call(typeof(Queryable), "Single", new Type[] { metaTable.EntityType }, whereCall);

            return singleCall;
        }

        public static MethodCallExpression BuildItemsQuery(IQueryable query, MetaTable metaTable, IList<MetaColumn> columns, string[] values)
        {
            // row
            var rowParam = Expression.Parameter(metaTable.EntityType, "row");
            // row.ID == 1
            var whereBody = BuildWhereBody(rowParam, columns, values);
            // row => row.ID == 1
            var whereLambda = Expression.Lambda(whereBody, rowParam);
            // Items.Where(row => row.ID == 1)
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

            return whereCall;
        }

        public static MethodCallExpression BuildWhereQuery(IQueryable query, MetaTable metaTable, MetaColumn column, string value)
        {
            // row
            var rowParam = Expression.Parameter(metaTable.EntityType, column.Name);
            // row.ID == 1
            var whereBody = BuildWhereBodyFragment(rowParam, column, value);
            // row => row.ID == 1
            var whereLambda = Expression.Lambda(whereBody, rowParam);
            // Items.Where(row => row.ID == 1)
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

            return whereCall;
        }

        public static MethodCallExpression BuildCustomQuery(IQueryable query, MetaTable metaTable, MetaColumn column, string value, QueryType queryType)
        {
            // row
            var rowParam = Expression.Parameter(metaTable.EntityType, metaTable.Name);

            // row.DisplayName
            var property = Expression.Property(rowParam, column.Name);

            // "prefix"
            var constant = Expression.Constant(value);

            // row.DisplayName.StartsWith("prefix")
            var startsWithCall = Expression.Call(property, typeof(string).GetMethod(queryType.ToString(), new Type[] { typeof(string) }), constant);

            // row => row.DisplayName.StartsWith("prefix")
            var whereLambda = Expression.Lambda(startsWithCall, rowParam);

            // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

            return whereCall;
        }

        //public static MethodCallExpression BuildCustomQuery(IQueryable query, MetaTable metaTable, MetaColumn column, string value)
        //{
        //    // row
        //    var rowParam = Expression.Parameter(metaTable.EntityType, metaTable.Name);

        //    // row.DisplayName
        //    var property = Expression.Property(rowParam, column.Name);

        //    // "prefix"
        //    var constant = Expression.Constant(value);

        //    // row.DisplayName.StartsWith("prefix")
        //    var startsWithCall = Expression.Call(property, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constant);

        //    // row => row.DisplayName.StartsWith("prefix")
        //    var whereLambda = Expression.Lambda(startsWithCall, rowParam);

        //    // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
        //    var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

        //    return whereCall;
        //}

        //public static MethodCallExpression BuildStartsWithQuery(IQueryable query, MetaTable metaTable, MetaColumn column, string value)
        //{
        //    // row
        //    var rowParam = Expression.Parameter(metaTable.EntityType, metaTable.Name);

        //    // row.DisplayName
        //    var property = Expression.Property(rowParam, column.Name);

        //    // "prefix"
        //    var constant = Expression.Constant(value);

        //    // row.DisplayName.StartsWith("prefix")
        //    var startsWithCall = Expression.Call(property, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constant);

        //    // row => row.DisplayName.StartsWith("prefix")
        //    var whereLambda = Expression.Lambda(startsWithCall, rowParam);

        //    // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
        //    var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

        //    return whereCall;
        //}

        // public static MethodCallExpression BuildEndsWithQuery(IQueryable query, MetaTable metaTable, MetaColumn column, string value)
        //{
        //    // row
        //    var rowParam = Expression.Parameter(metaTable.EntityType, metaTable.Name);

        //    // row.DisplayName
        //    var property = Expression.Property(rowParam, column.Name);

        //    // "prefix"
        //    var constant = Expression.Constant(value);

        //    // row.DisplayName.StartsWith("prefix")
        //    var startsWithCall = Expression.Call(property, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constant);

        //    // row => row.DisplayName.StartsWith("prefix")
        //    var whereLambda = Expression.Lambda(startsWithCall, rowParam);

        //    // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
        //    var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { metaTable.EntityType }, query.Expression, whereLambda);

        //    return whereCall;
        //}

        public static BinaryExpression BuildWhereBody(ParameterExpression parameter, IList<MetaColumn> columns, string[] values)
        {
            Debug.Assert(columns.Count == values.Length);
            Debug.Assert(columns.Count > 0);

            // row.ID == 1
            var whereBody = BuildWhereBodyFragment(parameter, columns[0], values[0]);
            for (int i = 1; i < values.Length; i++)
            {
                // row.ID == 1 && row.ID2 == 2
                whereBody = Expression.AndAlso(whereBody, BuildWhereBodyFragment(parameter, columns[i], values[i]));
            }

            return whereBody;
        }

        private static BinaryExpression BuildWhereBodyFragment(ParameterExpression parameter, MetaColumn column, string value)
        {
            // row.ID
            var property = Expression.Property(parameter, column.Name);
            // row.ID == 1
            return Expression.Equal(property, Expression.Constant(ChangeValueType(column, value)));
        }

        private static object ChangeValueType(MetaColumn column, string value)
        {
            if (column.ColumnType == typeof(Guid))
            {
                return new Guid(value);
            }
            else
            {
                return Convert.ChangeType(value, column.TypeCode, CultureInfo.InvariantCulture);
            }
        }
    }
}