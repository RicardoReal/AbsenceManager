﻿using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;

namespace AbsenceManager.DynamicData.CustomPages.Roles
{
    public partial class Edit : System.Web.UI.Page
    {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e)
        {
            table = DynamicDataRouteHandler.GetRequestMetaTable(Context);
            FormView1.SetMetaTable(table);
            DetailsDataSource.EntityTypeFilter = table.EntityType.Name;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = table.DisplayName;
            DetailsDataSource.Include = table.ForeignKeyColumnsNames;
        }

        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == DataControlCommands.CancelCommandName)
            {
                Response.Redirect(table.ListActionPath);
            }
        }

        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
            {
                Response.Redirect(table.ListActionPath);
            }
            else
            {
                if (e.Exception.GetType() == typeof(ValidationException))
                {
                    customValida.ErrorMessage = e.Exception.Message;
                    customValida.IsValid = false;
                    e.ExceptionHandled = true;
                }
            }
        }

    }
}
