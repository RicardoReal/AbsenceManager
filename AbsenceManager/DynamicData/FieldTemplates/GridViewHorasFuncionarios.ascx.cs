using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AbsenceManager
{
    public partial class GridViewHorasFuncionariosField : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected System.Web.DynamicData.MetaTable table, tableInsert;

        protected void Page_Init(object sender, EventArgs e)
        {
            var metaChildColumn = Column as MetaChildrenColumn;
            var metaForeignKeyColumn = metaChildColumn.ColumnInOtherTable as MetaForeignKeyColumn;

            if (metaChildColumn != null && metaForeignKeyColumn != null)
            {
                #region GridView/Update
                // Definir contexto/entidade
                GridDataSource.ContextTypeName = metaChildColumn.ChildTable.DataContextType.FullName;
                GridDataSource.EntitySetName = metaChildColumn.ChildTable.Name;

                // Definir update/delete/insert
                GridDataSource.EnableDelete = false;
                GridDataSource.EnableInsert = false;
                GridDataSource.EnableUpdate = false;

                // Get de uma instancia da MetaTable
                table = GridDataSource.GetTable();

                // Definir as datakeys da Gridview
                String[] keys = new String[metaChildColumn.ChildTable.PrimaryKeyColumns.Count];
                int i = 0;
                foreach (var keyColumn in metaChildColumn.ChildTable.PrimaryKeyColumns)
                {
                    keys[i] = keyColumn.Name;
                    i++;
                }
                GridView1.DataKeyNames = keys;

                // Gerar as colunas da gridview com o FieldTemplateRowGenerator
                // Filtrar a relação
                GridView1.ColumnsGenerator = new FieldTemplateRowGenerator(table, new string[] { metaForeignKeyColumn.Name });
                //GridView1.ColumnsGenerator = new FieldTemplateRowGenerator(table);

                // Filtros, FKs, Where clause
                GridDataSource.EntityTypeFilter = table.EntityType.Name;
                GridDataSource.Include = table.ForeignKeyColumnsNames;
                GridDataSource.AutoGenerateWhereClause = true;
                #endregion

                #region Script
                ScriptContainer.InnerHtml = @"
                        <script>
                            $(function () {
                                $('.DDDropDown[id]').each(
                                        function (index) {
                                            var id = $(this).attr('id');

                                            if (id.indexOf('_" + table.Name + @"_') > 0 && id.indexOf('_" + metaForeignKeyColumn.Name + @"_') > 0) {
                                                $(this).parent().parent().css('display', 'none');
                                                $(this).parent().parent().css('visibility', 'hidden');
                                            }
                                        }
                                    )
                            });
                        </script>";
                #endregion
            }
            else
            {
                // throw an error if set on column other than MetaChildrenColumns
                throw new InvalidOperationException("The GridView FieldTemplate can only be used with MetaChildrenColumns.");
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            //GridView
            var metaChildrenColumn = Column as MetaChildrenColumn;
            var metaForeignKeyColumn = metaChildrenColumn.ColumnInOtherTable as MetaForeignKeyColumn;
            int i = 0;

            foreach (String fkName in metaForeignKeyColumn.ForeignKeyNames)
            {
                var fkColumn = metaChildrenColumn.ChildTable.GetColumn(fkName);

                var param = new Parameter();
                param.Name = fkColumn.Name;
                param.Type = fkColumn.TypeCode;
                param.DefaultValue = Request.QueryString[i];

                GridDataSource.WhereParameters.Add(param);

                i++;
            }
        }

        // Não mostar a FormView no momento em que se edita um registo da GridView
        protected void GridView1_RowEditing(object sender, EventArgs e)
        {
        }

        protected void GridView1_SelectedIndexChanging(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
        }

        // Refresh da GridView após um insert
        protected void FormViewDataSource_Inserted(object sender, EntityDataSourceChangedEventArgs e)
        {
            GridView1.DataBind();
        }

        public override Control DataControl
        {
            get
            {
                return GridView1;
            }
        }

    }
}
