using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.UI;
using System.Globalization;

namespace AbsenceManager
{
    public partial class JQueryCalendarDateField : System.Web.DynamicData.FieldTemplateUserControl
    {
        DateTime dateTimeComp;

        public override string FieldValueString
        {
            get
            {
                string value = base.FieldValueString;

                if (GetModeAttribute() == "Time")
                {
                    if (FieldValue != null)
                    {
                        DateTime startDate = (DateTime)FieldValue;
                        string format = "HH:mm";
                        string result = startDate.ToString(format);

                        if (startDate != null && dateTimeComp != null)
                        {
                            // TODO: Alterado para comparar com a data actual. Tornar genérico.
                            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
                            TimeSpan dateDiff = startDate.Subtract(endDate);

                            if (dateDiff.Days != 0)
                                result += " (" + ((dateDiff.Days < 0) ? "-" : "+") + Math.Abs(dateDiff.Days).ToString() + ")";
                        }

                        value = result;
                    }
                }

                if (GetModeAttribute() == "Date")
                {
                    if (FieldValue != null)
                    {
                        DateTime startDate = (DateTime)FieldValue;
                        string format = "yyyy-MM-dd";
                        string result = startDate.ToString(format);

                        value = result;
                    }
                }

                return value;
            }
        }

        public override Control DataControl
        {
            get
            {
                return Literal1;
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            if (GetModeAttribute() == "Time")
            {
                // Get da Entity
                var entity = EntityDataSourceExtensions.GetEntity(Row);

                // Se a Entity contem o Field
                if (GetField(entity) != null)
                {
                    dateTimeComp = (DateTime)GetField(entity);
                }
                else
                {
                    // Se não contiver procurar no FieldClass
                    entity = GetFieldClass(entity);

                    if (entity != null)
                    {
                        if (GetField(entity) != null)
                        {
                            dateTimeComp = (DateTime)GetField(entity);
                        }
                    }
                }
            }
        }

        private object GetField(object entity)
        {
            if (entity != null)
            {
                PropertyInfo pi = entity.GetType().GetProperty(GetFieldAttribute());

                if (pi != null)
                    return pi.GetValue(entity, null);
            }

            return null;
        }

        private string GetFieldAttribute()
        {
            return ((UIHintAttribute)this.Column.Attributes[typeof(UIHintAttribute)]).ControlParameters["Field"].ToString();
        }

        private object GetFieldClass(object entity)
        {
            if (entity != null)
            {
                PropertyInfo pi = entity.GetType().GetProperty(GetFieldClassAttribute());

                if (pi != null)
                    return pi.GetValue(entity, null);
            }

            return null;
        }

        private string GetFieldClassAttribute()
        {
            return ((UIHintAttribute)this.Column.Attributes[typeof(UIHintAttribute)]).ControlParameters["FieldClass"].ToString();
        }

        private string GetModeAttribute()
        {
            return ((UIHintAttribute)this.Column.Attributes[typeof(UIHintAttribute)]).ControlParameters["PresentationMode"].ToString();
        }
    }
}
