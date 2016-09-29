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
    public partial class PasswordField : System.Web.DynamicData.FieldTemplateUserControl
    {
        //private const int MAX_DISPLAYLENGTH_IN_LIST = 25;

        public override string FieldValueString
        {
            get
            {
                string value = base.FieldValueString;

                AMMembershipProvider gmp = (AMMembershipProvider)Membership.Provider;

                value = "******";

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

    }
}
