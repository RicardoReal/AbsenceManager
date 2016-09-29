using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.DynamicData.ModelProviders;

namespace AbsenceManager.Security
{
    public class SecureMetaModel : MetaModel
    {
        /// <summary>
        /// Delegate to allow custom column generator to be passed in.
        /// </summary>
        public delegate IEnumerable<MetaColumn> GetVisibleColumns(IEnumerable<MetaColumn> columns);
        public delegate IEnumerable<MetaColumn> GetVisibleFilters(IEnumerable<MetaColumn> filters);

        private GetVisibleColumns _getVisibleColumns;
        private GetVisibleFilters _getVisibleFilters;


        public SecureMetaModel() { }

        public SecureMetaModel(GetVisibleColumns getVisibleColumns)
        {
            _getVisibleColumns = getVisibleColumns;
        }

        public SecureMetaModel(GetVisibleFilters getVisibleFilters)
        {
            _getVisibleFilters = getVisibleFilters;
        }

        public SecureMetaModel(GetVisibleColumns getVisibleColumns, GetVisibleFilters getVisibleFilters)
        {
            _getVisibleColumns = getVisibleColumns;
            _getVisibleFilters = getVisibleFilters;
        }


        protected override MetaTable CreateTable(TableProvider provider)
        {
            if (_getVisibleColumns == null && _getVisibleFilters == null)
                return new SecureMetaTable(this, provider);
            else if (_getVisibleColumns != null && _getVisibleFilters == null)
                return new SecureMetaTable(this, provider, _getVisibleColumns);
            else if (_getVisibleColumns == null && _getVisibleFilters != null)
                return new SecureMetaTable(this, provider, _getVisibleFilters);
            else
                return new SecureMetaTable(this, provider, _getVisibleColumns, _getVisibleFilters);
        }
    }
}