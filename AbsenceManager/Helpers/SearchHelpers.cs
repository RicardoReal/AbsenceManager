using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using System.Configuration;
using System.Web.Configuration;
using System.Text;
using System.Linq.Expressions;

namespace AbsenceManager
{
    public class SearchHelpers
    {
        // Configuração do Search através do web.config
        public static bool ConfigureSearch(string entityName, ref SearchExpression se)
        {
            Configuration siteCfg = WebConfigurationManager.OpenWebConfiguration("~/");
            string searchPrefix = "Search-";
            KeyValueConfigurationElement kvCfg = siteCfg.AppSettings.Settings[searchPrefix + entityName];
            string searchCfg = null;

            if (kvCfg != null)
            {
                searchCfg = kvCfg.Value;
                se.DataFields = searchCfg;

                return true;
            }

            return false;
        }

        // Configuração da tooltip do Search através do web.config
        public static string ConfigureTooltipText(string entityName)
        {
            Configuration siteCfg = WebConfigurationManager.OpenWebConfiguration("~/");
            string searchPrefix = "Search-";
            //string tooltipPrefix = "Expand filters - ";
            string tooltipPrefix = "";
            KeyValueConfigurationElement kvCfg = siteCfg.AppSettings.Settings[searchPrefix + entityName];
            string res = "";

            if (kvCfg != null)
            {
                res = kvCfg.Value;

                StringBuilder builder = new StringBuilder();

                foreach (char c in res)
                {
                    if (Char.IsUpper(c) && builder.Length > 0)
                        builder.Append(' ');

                    if (c == '.' && builder.Length > 0)
                        builder.Append(" ->");
                    else
                        builder.Append(c);
                }

                res = tooltipPrefix + builder.ToString();
            }

            return res;
        }

        // Filtro das Airlines
        //public static IQueryable GetAirlineFilter(IQueryable source)
        //{
        //    string userName = System.Web.Security.Membership.GetUser().UserName;

        //    if (SearchHelpers.IsHandlerLimitated(userName))
        //    {
        //        // Se  o utilizador estiver limitado nos handlers, buscar as airlines desses handlers
        //        GOPadsMembershipProvider gomp = (GOPadsMembershipProvider)System.Web.Security.Membership.Provider;
        //        long[] airlineList = gomp.GetAirlineLimitationsForUser(userName);
        //        List<long> airlineListGeneric = airlineList.ToList<long>();

        //        // Construir a expression tree para a where clause
        //        // Flight ou Flight(Infraestrutura)
        //        ParameterExpression entityParameter = Expression.Parameter(source.ElementType);
        //        ConstantExpression airlinesParameter = Expression.Constant(airlineListGeneric, typeof(List<long>));

        //        MemberExpression memberExpression;

        //        if (source.ElementType == typeof(Flight))
        //        {
        //            memberExpression = Expression.Property(entityParameter, "AirlineID");
        //        }
        //        else
        //        {
        //            memberExpression = Expression.Property(Expression.Property(entityParameter, "Flight"), "AirlineID");
        //        }

        //        Expression convertExpression = Expression.Convert(memberExpression, typeof(long));
        //        MethodCallExpression containsExpression = Expression.Call(airlinesParameter
        //                , "Contains", new Type[] { }, convertExpression);

        //        LambdaExpression lambda = Expression.Lambda(containsExpression, entityParameter);

        //        MethodCallExpression where = Expression.Call(
        //          typeof(Queryable),
        //          "Where",
        //          new Type[] { source.ElementType },
        //          new Expression[] { source.Expression, Expression.Quote(lambda) });

        //        source = source.Provider.CreateQuery(where);
        //    }

        //    return source;
        //}

        //// Filtro do MovementType
        //public static IQueryable GetFlightMovementTypeFilter(IQueryable source, string valueString)
        //{
        //    if (valueString != "")
        //    {
        //        // Construir a expression tree para a where clause
        //        long? value = Int64.Parse(valueString);
        //        ParameterExpression entityParameter = Expression.Parameter(source.ElementType);
        //        ConstantExpression valueParameter = Expression.Constant(value, typeof(long));
        //        MemberExpression memberExpression = Expression.Property(entityParameter, "MovementTypeID");
        //        BinaryExpression comparisonExpression = Expression.Equal(memberExpression, valueParameter);

        //        LambdaExpression lambda = Expression.Lambda(comparisonExpression, entityParameter);

        //        MethodCallExpression where = Expression.Call(
        //          typeof(Queryable),
        //          "Where",
        //          new Type[] { source.ElementType },
        //          new Expression[] { source.Expression, Expression.Quote(lambda) });

        //        source = source.Provider.CreateQuery(where);
        //    }

        //    return source;
        //}

        //public static IQueryable GetFlightInfraMovementTypeFilter(IQueryable source, string valueString)
        //{
        //    if (valueString != "")
        //    {
        //        // Construir a expression tree para a where clause
        //        long? value = Int64.Parse(valueString);
        //        ParameterExpression entityParameter = Expression.Parameter(source.ElementType);
        //        ConstantExpression valueParameter = Expression.Constant(value, typeof(long));
        //        MemberExpression memberExpression = Expression.Property(Expression.Property(entityParameter, "Flight"), "MovementTypeID");
        //        BinaryExpression comparisonExpression = Expression.Equal(memberExpression, valueParameter);

        //        LambdaExpression lambda = Expression.Lambda(comparisonExpression, entityParameter);

        //        MethodCallExpression where = Expression.Call(
        //          typeof(Queryable),
        //          "Where",
        //          new Type[] { source.ElementType },
        //          new Expression[] { source.Expression, Expression.Quote(lambda) });

        //        source = source.Provider.CreateQuery(where);
        //    }

        //    return source;
        //}

        public static IQueryable GetActiveFilter(IQueryable source, string valueString)
        {
            if (valueString != "")
            {
                // Construir a expression tree para a where clause
                //bool? value = Boolean.Parse(valueString);
                bool value = (valueString == "1");
                ParameterExpression entityParameter = Expression.Parameter(source.ElementType);
                ConstantExpression valueParameter = Expression.Constant(value, typeof(bool));
                MemberExpression memberExpression = Expression.Property(entityParameter, "Active");
                BinaryExpression comparisonExpression = Expression.Equal(memberExpression, valueParameter);

                LambdaExpression lambda = Expression.Lambda(comparisonExpression, entityParameter);

                MethodCallExpression where = Expression.Call(
                  typeof(Queryable),
                  "Where",
                  new Type[] { source.ElementType },
                  new Expression[] { source.Expression, Expression.Quote(lambda) });

                source = source.Provider.CreateQuery(where);
            }

            return source;
        }

        public static IQueryable GetTrackChangesFilter(IQueryable source, string entityID, string propertyName)
        {
            if (!String.IsNullOrEmpty(entityID))
            {
                // Construir a expression tree para a where clause
                long? entityIDLong = Int64.Parse(entityID);

                ParameterExpression entityParameter = Expression.Parameter(source.ElementType);

                ConstantExpression entityIDParameter = Expression.Constant(entityIDLong, typeof(long));
                MemberExpression entityIDExpression = Expression.Property(entityParameter, propertyName);
                BinaryExpression entityIDComparison = Expression.Equal(entityIDExpression, entityIDParameter);

                LambdaExpression lambda = Expression.Lambda(entityIDComparison, entityParameter);

                MethodCallExpression where = Expression.Call(
                  typeof(Queryable),
                  "Where",
                  new Type[] { source.ElementType },
                  new Expression[] { source.Expression, Expression.Quote(lambda) });

                source = source.Provider.CreateQuery(where);
            }

            return source;
        }
    }
}