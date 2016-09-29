using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;

namespace AbsenceManager
{
    public partial class DateEqualsFilter : System.Web.DynamicData.QueryableFilterUserControl
    {
        public string DateFrom
        {
            get { return TextBoxDateFrom.Text; }
        }

        public override Control FilterControl
        {
            get
            {
                return TextBoxDateFrom;
            }
        }

        public void Page_Init(object sender, EventArgs e)
        {
            if (!Column.ColumnType.Equals(typeof(DateTime)))
                throw new InvalidOperationException(String.Format("A date range filter was loaded for column '{0}' but the column has an incompatible type '{1}'.",
                    Column.Name, Column.ColumnType));

            if (DefaultValue != null)
                TextBoxDateFrom.Text = DefaultValue;

            if (String.IsNullOrEmpty(TextBoxDateFrom.Text))
            {
                FilterUIHintAttribute fuha = ((FilterUIHintAttribute)this.Column.Attributes[typeof(FilterUIHintAttribute)]);

                if (fuha != null)
                {
                    if (fuha.ControlParameters.Count > 0)
                    {
                        var cp = fuha.ControlParameters["Mode"];
                        string mode = cp.ToString();

                        TextBoxDateFrom.Text = GetDefaultValue(mode);
                    }
                }
            }
        }

        public override IQueryable GetQueryable(IQueryable source)
        {
            DateTime mDateTime;

            bool IsDateTime = DateTime.TryParse(DateFrom, out mDateTime);

            if (!IsDateTime)
            {
                TextBoxDateFrom.Text = "";
                return source;
            }

            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, "item");
            Expression body = BuildQueryBody(parameterExpression);

            LambdaExpression lambda = Expression.Lambda(body, parameterExpression);
            MethodCallExpression whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery(whereCall);
        }

        private Expression BuildQueryBody(ParameterExpression parameterExpression)
        {
            Expression propertyExpression = ExpressionHelper.GetValue(ExpressionHelper.CreatePropertyExpression(parameterExpression, Column.Name));
            TypeConverter converter = TypeDescriptor.GetConverter(Column.ColumnType);
            return BuildCompareExpression(parameterExpression, propertyExpression, converter.ConvertFromString(DateFrom), Expression.Equal);
        }

        private Expression BuildCompareExpression(Expression parameterExpression, Expression propertyExpression, object value, Func<Expression, Expression, BinaryExpression> comparisonFunction)
        {
            ConstantExpression valueExpression = Expression.Constant(value);
            BinaryExpression comparison = comparisonFunction(propertyExpression, valueExpression);

            // Não filta as entradas que tenham a coluna a NULL
            Expression convertExpression = Expression.Convert(propertyExpression, typeof(DateTime?));
            BinaryExpression comparison2 = Expression.Equal(convertExpression, Expression.Constant((DateTime?)null));

            Expression orElseExpression = Expression.OrElse(comparison, comparison2);

            return orElseExpression;
        }

        protected void TextBoxDateFrom_TextChanged(object sender, EventArgs e)
        {
            OnFilterChanged();
            Page.AddFilterValueToSession(Column, TextBoxDateFrom.Text);
        }

        private string GetDefaultValue(string mode)
        {
            string result = "";
            DateTime defaultDateTime = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm";
            string settingsPrefix = "GreaterThanFilter";

            // Modo nos settings da Application
            if (mode == "BaggageBelt" || mode == "Gate" || mode == "Stand" || mode == "CheckIn" || mode == "Chute")
            {
                string modeDB = (string)Application.Get(settingsPrefix + mode);

                if (modeDB.Contains("Today"))
                {
                    defaultDateTime = new DateTime(defaultDateTime.Year, defaultDateTime.Month, defaultDateTime.Day);
                    result = defaultDateTime.ToString(format);
                }

                if (modeDB.Contains("MinusMinutes"))
                {
                    int minutes = Int32.Parse(modeDB.Split('_')[1]);
                    defaultDateTime = defaultDateTime.Subtract(new TimeSpan(0, minutes, 0));
                    result = defaultDateTime.ToString(format);
                }
            }
            else if (mode == "NowMinus1Hour")
            {
                defaultDateTime = defaultDateTime.Subtract(new TimeSpan(1, 0, 0));
                result = defaultDateTime.ToString(format);
            }
            else if (mode == "Today")
            {
                defaultDateTime = new DateTime(defaultDateTime.Year, defaultDateTime.Month, defaultDateTime.Day);
                result = defaultDateTime.ToString(format);
            }
            else if (mode == "FlightsMinutesFlightsAfter" || mode == "GatesMinutesOpenTime" || mode == "BeltsMinutesOpenTime" || mode == "CheckInsMinutesOpenTime" || mode == "ChutesMinutesOpenTime" || mode == "StandsMinutesONB")
            {
                defaultDateTime = defaultDateTime.Subtract(new TimeSpan(0, System.Convert.ToInt32(Application.Get(mode)), 0));
                result = defaultDateTime.ToString(format);
            }

            return result;
        }
    }
}
