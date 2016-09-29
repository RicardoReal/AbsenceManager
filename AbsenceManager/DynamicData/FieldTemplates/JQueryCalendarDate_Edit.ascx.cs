using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Reflection;

namespace AbsenceManager
{
    public partial class JQueryCalendarDate_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        private static DataTypeAttribute DefaultDateAttribute = new DataTypeAttribute(DataType.DateTime);

        private int year
        {
            get { return ((DateTime)base.FieldValue).Year; }
        }

        private int month
        {
            get { return ((DateTime)base.FieldValue).Month - 1; }
        }

        private int day
        {
            get { return ((DateTime)base.FieldValue).Day; }
        }

        private int hour
        {
            get { return ((DateTime)base.FieldValue).Hour; }
        }

        private int minute
        {
            get { return ((DateTime)base.FieldValue).Minute; }
        }

        public string dateToBind
        {
            get
            {
                string value = "";

                if (base.FieldValue != null)
                {
                    value = year.ToString() + "," + month.ToString() + "," + day.ToString() + "," + hour.ToString() + "," + minute.ToString() + ",0";
                }

                return value;
            }
        }

        DateTime dateTimeComp;

        public override string FieldValueString
        {
            get
            {
                string value = base.FieldValueString;

                string format = "yyyy-MM-dd HH:mm";

                if (FieldValue != null)
                {
                    DateTime startDate = (DateTime)FieldValue;
                    value = startDate.ToString(format);
                }

                /*if (value == "") { 
                    DateTime startDate = DateTime.Now;
                    value = startDate.ToString(format);
                }*/

                return value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.ToolTip = Column.Description;

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(DynamicValidator1);
            SetUpCustomValidator(DateValidator);
        }

        protected override void OnDataBinding(EventArgs e)
        {
            if (GetROAtribute())
            {
                TextBox1.Visible = false;
            }
            else
            {
                Literal1.Visible = false;
            }

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

        private void SetUpCustomValidator(CustomValidator validator)
        {
            if (Column.DataTypeAttribute != null)
            {
                switch (Column.DataTypeAttribute.DataType)
                {
                    case DataType.Date:
                    case DataType.DateTime:
                    case DataType.Time:
                        validator.Enabled = true;
                        DateValidator.ErrorMessage = HttpUtility.HtmlEncode(Column.DataTypeAttribute.FormatErrorMessage(Column.DisplayName));
                        break;
                }
            }
            else if (Column.ColumnType.Equals(typeof(DateTime)))
            {
                validator.Enabled = true;
                DateValidator.ErrorMessage = HttpUtility.HtmlEncode(DefaultDateAttribute.FormatErrorMessage(Column.DisplayName));
            }
        }

        protected void DateValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime dummyResult;
            args.IsValid = DateTime.TryParse(args.Value, out dummyResult);
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            dictionary[Column.Name] = ConvertEditedValue(TextBox1.Text);
        }

        public override Control DataControl
        {
            get
            {
                return TextBox1;
            }
        }

        private bool GetROAtribute()
        {
            UIHintAttribute uha = ((UIHintAttribute)this.Column.Attributes[typeof(UIHintAttribute)]);

            object cp = null;
            if (uha.ControlParameters.ContainsKey("ROEdit"))
                cp = uha.ControlParameters["ROEdit"];

            return cp == null ? false : (cp.ToString() == "True");
        }

        private object GetField(object entity)
        {
            PropertyInfo pi = entity.GetType().GetProperty(GetFieldAttribute());

            if (pi != null)
                return pi.GetValue(entity, null);

            return null;
        }

        private string GetFieldAttribute()
        {
            return ((UIHintAttribute)this.Column.Attributes[typeof(UIHintAttribute)]).ControlParameters["Field"].ToString();
        }

        private object GetFieldClass(object entity)
        {
            PropertyInfo pi = entity.GetType().GetProperty(GetFieldClassAttribute());

            if (pi != null)
                return pi.GetValue(entity, null);

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
