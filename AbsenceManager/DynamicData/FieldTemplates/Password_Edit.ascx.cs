using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace AbsenceManager
{
    public partial class Password_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        public override string FieldValueEditString
        {
            get
            {
                string value = base.FieldValueEditString;

                if (value.Length > 0)
                {
                    AMMembershipProvider gmp = (AMMembershipProvider)Membership.Provider;
                    value = "******";
                }

                TextBox1.Attributes.Add("value", value);  

                return value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Column.MaxLength < 20)
            {
                TextBox1.Columns = Column.MaxLength;
            }
            TextBox1.ToolTip = Column.Description;

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(DynamicValidator1);
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            if (Column.MaxLength > 0)
            {
                TextBox1.MaxLength = Math.Max(FieldValueEditString.Length, Column.MaxLength);
            }
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
