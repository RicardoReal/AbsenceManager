using System;
using System.Collections.Specialized;
using System.Web.DynamicData;
using System.Linq;
using System.Web.UI;
using System.Collections;
using AbsenceManager.DynamicData.Filters.Helpers;

namespace AbsenceManager
{
    public partial class AutocompleteField_Edit : System.Web.DynamicData.FieldTemplateUserControl
    {
        private new MetaForeignKeyColumn Column
        {
            get { return (MetaForeignKeyColumn) base.Column; }
        }

        public override Control DataControl
        {
            get
            {
                return AutocompleteValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(DynamicValidator1);
        }

        protected override void OnDataBinding(EventArgs e)
        {
            var fkColumn = Column as MetaForeignKeyColumn;

            //// dynamically build the context key so the web service knows which table we're talking about
            //autoComplete1.ContextKey = fkColumn.ParentTable.Provider.DataModel.ContextType.FullName + "#" + fkColumn.ParentTable.Name;
            autoComplete1.ContextKey = AutocompleteFieldService.GetContextKey(fkColumn.ParentTable);

            // add JavaScript to create post-back when user selects an item in the list
            var method = "function(source, eventArgs) {\r\n" +
            "var valueField = document.getElementById('" + AutocompleteValue.ClientID + "');\r\n" +
            "valueField.value = eventArgs.get_value();\r\n" +
            "}";
            autoComplete1.OnClientItemSelected = method;

            // modify behaviorID so it does not clash with other autocomplete extenders on the page
            autoComplete1.Animations = autoComplete1.Animations.Replace(autoComplete1.BehaviorID, AutocompleteTextBox.UniqueID);
            autoComplete1.BehaviorID = AutocompleteTextBox.UniqueID;

            string _FieldValueString = GetSelectedValueString();

            if (!String.IsNullOrEmpty(_FieldValueString))
            {
                // set the initial value of the field if it's present in the request URL

                MetaTable parentTable = fkColumn.ParentTable;
                IQueryable query = parentTable.GetQuery();
                // multi-column PK values are separated by commas
                var singleCall = LinqExpressionHelper.BuildSingleItemQuery(query, parentTable, _FieldValueString.Split(','));
                var row = query.Provider.Execute(singleCall);
                string display = parentTable.GetDisplayString(row);

                AutocompleteTextBox.Text = display;
                AutocompleteValue.Value = _FieldValueString;
            }
        }

        protected void AutocompleteTextBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(AutocompleteTextBox.Text))
            {
                AutocompleteValue.Value = "";
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            // If it's an empty string, change it to null
            string value = AutocompleteValue.Value;
            if (String.IsNullOrEmpty(value))
            {
                value = null;
            }

            ExtractForeignKey(dictionary, value);
        }

    }
}