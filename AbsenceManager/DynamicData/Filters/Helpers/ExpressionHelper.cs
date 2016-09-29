using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Globalization;
using System.ComponentModel;
using System.Web.DynamicData;

namespace AbsenceManager
{
    public static class ExpressionHelper
    {
        public static Expression GetValue(Expression exp)
        {
            Type realType = GetUnderlyingType(exp.Type);
            if (realType == exp.Type)
                return exp;

            return Expression.Convert(exp, realType);
        }

        private static Type RemoveNullableFromType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static Type GetUnderlyingType(Type type)
        {
            return RemoveNullableFromType(type);
        }

        public static Expression Join(IEnumerable<Expression> expressions, Func<Expression, Expression, Expression> joinFunction)
        {
            Expression result = null;
            foreach (Expression e in expressions)
            {
                if (e == null)
                    continue;

                if (result == null)
                    result = e;
                else
                    result = joinFunction(result, e);
            }
            return result;
        }

        public static IQueryable ApplyInequalityFilter(IQueryable source, string propertyName, object value)
        {
            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, string.Empty);
            Expression expression = CreatePropertyExpression(parameterExpression, propertyName);
            if ((Nullable.GetUnderlyingType(expression.Type) != null) && (value != null))
            {
                expression = Expression.Convert(expression, Nullable.GetUnderlyingType(expression.Type) ?? expression.Type);
            }
            value = ChangeType(value, expression.Type);
            LambdaExpression expression4 = Expression.Lambda(Expression.NotEqual(expression, Expression.Constant(value)),

                new ParameterExpression[] { parameterExpression });
            MethodCallExpression expression5 = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType },

                new Expression[] { source.Expression, Expression.Quote(expression4) });
            return source.Provider.CreateQuery(expression5);
        }

        public static Expression CreatePropertyExpression(Expression parameterExpression, string propertyName)
        {
            Expression propExpression = null;
            string[] props = propertyName.Split('.');
            foreach (var p in props)
            {
                if (propExpression == null)
                    propExpression = Expression.PropertyOrField(parameterExpression, p);
                else
                    propExpression = Expression.PropertyOrField(propExpression, p);
            }
            return propExpression;
        }

        private static bool TypeAllowsNull(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
        }

        public static object ChangeType(object value, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (value == null)
            {
                if (TypeAllowsNull(type))
                    return null;
                return Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
            }

            if (value.GetType() == type)
                return value;

            type = RemoveNullableFromType(type);

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(value.GetType()))
                return converter.ConvertFrom(value);

            TypeConverter otherConverter = TypeDescriptor.GetConverter(value.GetType());
            if (otherConverter.CanConvertTo(type))
                return otherConverter.ConvertTo(value, type);

            throw new InvalidOperationException(String.Format("Cannot convert type '{0}' to '{1}'", value.GetType(), type));
        }

        public static MethodCallExpression BuildSingleItemQuery(this IQueryable query, MetaTable metaTable, string[] primaryKeyValues)
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

        public static BinaryExpression BuildWhereBody(ParameterExpression parameter, IList<MetaColumn> columns, string[] values)
        {
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
                return new Guid(value);
            else
                return Convert.ChangeType(value, column.TypeCode, CultureInfo.InvariantCulture);
        }
    }
}