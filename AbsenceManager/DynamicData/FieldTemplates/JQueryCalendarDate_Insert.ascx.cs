﻿using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace AbsenceManager
{
    public partial class JQueryCalendarDate_InsertField : System.Web.DynamicData.FieldTemplateUserControl
    {
        private static DataTypeAttribute DefaultDateAttribute = new DataTypeAttribute(DataType.DateTime);

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.ToolTip = Column.Description;

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(DynamicValidator1);
            SetUpCustomValidator(DateValidator);
        }

        public override string FieldValueEditString
        {
            get
            {
                string value = base.FieldValueEditString;

                if (FieldValue != null)
                {
                    DateTime startDate = (DateTime)FieldValue;
                    string format = "yyyy-MM-dd HH:mm";
                    value = startDate.ToString(format);
                }

                return value;
            }
        }

        private string GetModeAttribute()
        {
            return ((UIHintAttribute)this.Column.Attributes[typeof(UIHintAttribute)]).ControlParameters["PresentationMode"].ToString();
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
    }
}
