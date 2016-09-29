using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using System.Collections;

public class FieldTemplateRowGenerator : IAutoFieldGenerator
{
    struct MetaColumnOrdered
    {
        public MetaColumn Column;
        public int Order;

        public MetaColumnOrdered(MetaColumn _Column, int _Order)
        {
            this.Column = _Column;
            this.Order = _Order;
        }
    }

    protected MetaTable _table;
    protected String[] _hideColumns;

    public FieldTemplateRowGenerator(MetaTable table)
    {
        _table = table;
    }

    public FieldTemplateRowGenerator(MetaTable table, String[] hideColumns)
    {
        _table = table;
        _hideColumns = hideColumns;
    }

    public ICollection GenerateFields(Control control)
    {
        List<MetaColumnOrdered> _MetaColumns = new List<MetaColumnOrdered>();

        foreach (var _column in _table.Columns)
        {
            if (!_column.Scaffold || _column.IsLongString)
                continue;

            if (_hideColumns != null && _hideColumns.Contains(_column.Name))
                continue;

            // If the column doesn't have an order, then it must be with order 0.
            int? _Order = 0;
            if (_column.Attributes[typeof(DisplayAttribute)] != null)
            {
                _Order = ((DisplayAttribute)_column.Attributes[typeof(DisplayAttribute)]).GetOrder();
                if (_Order == null) _Order = ((DisplayAttribute)_column.Attributes[typeof(DisplayAttribute)]).Order = 0;
            }
            else _Order = 0;

            _MetaColumns.Add(new MetaColumnOrdered(_column, Convert.ToInt32(_Order)));
        }

        // Sort Meta Columns List by Order.
        _MetaColumns.Sort((_column1, _column2) => _column1.Order.CompareTo(_column2.Order));

        List<DynamicField> _Fields = new List<DynamicField>();

        foreach (var _column in _MetaColumns)
        {
            DynamicField f = new DynamicField();

            f.DataField = _column.Column.Name;
            _Fields.Add(f);
        }

        return _Fields;
    }
}
