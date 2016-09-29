using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.Web.DynamicData.ModelProviders;

namespace AbsenceManager.Security
{
    public class SecureMetaTable : MetaTable
    {
        private SecureMetaModel.GetVisibleColumns _getVisibleColumns;
        private SecureMetaModel.GetVisibleFilters _getVisibleFilters;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMetaTable"/> class.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <param name="tableProvider">The table provider.</param>
        public SecureMetaTable(
            MetaModel metaModel,
            TableProvider tableProvider) :
            base(metaModel, tableProvider) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMetaTable"/> class.
        /// </summary>
        /// <param name="metaModel">The meta model.</param>
        /// <param name="tableProvider">The table provider.</param>
        /// <param name="getVisibleColumns">Delegate to get the visible columns.</param>
        public SecureMetaTable(
        MetaModel metaModel,
        TableProvider tableProvider,
        SecureMetaModel.GetVisibleColumns getVisibleColumns) :
            base(metaModel, tableProvider)
        {
            _getVisibleColumns = getVisibleColumns;
        }

        public SecureMetaTable(MetaModel metaModel, TableProvider tableProvider,
        SecureMetaModel.GetVisibleFilters getVisibleFilters) :
            base(metaModel, tableProvider)
        {
            _getVisibleFilters = getVisibleFilters;
        }

        public SecureMetaTable(MetaModel metaModel, TableProvider tableProvider,
        SecureMetaModel.GetVisibleColumns getVisibleColumns, SecureMetaModel.GetVisibleFilters getVisibleFilters) :
            base(metaModel, tableProvider)
        {
            _getVisibleColumns = getVisibleColumns;
            _getVisibleFilters = getVisibleFilters;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        public override IEnumerable<MetaColumn> GetScaffoldColumns(DataBoundControlMode mode, ContainerType containerType)
        {
            if (_getVisibleColumns == null)
                return base.GetScaffoldColumns(mode, containerType);
            else
                return _getVisibleColumns(base.GetScaffoldColumns(mode, containerType));
        }

        public override IEnumerable<MetaColumn> GetFilteredColumns()
        {
            if (_getVisibleFilters == null)
                return base.GetFilteredColumns();
            else
                return _getVisibleFilters(base.GetFilteredColumns());
        }
    }
}