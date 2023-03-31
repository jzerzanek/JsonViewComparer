using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using JsonViewComparer.Controls;
using JsonViewComparer.Enums;

namespace JsonViewComparer.Entities;

internal class JWCObjectArray : JWCBase
{
    public List<JWCObject> Items { get; set; }

    public DataTable DataTable
    {
        get
        {
            var dataTable = new DataTable();
            this.Columns.Clear();
            var propertyCellTemplate = JsonViewComparerControl.JWCPropertyCellTemplate;
            var simpleArrayCellTemplate = JsonViewComparerControl.JWCSimpleArrayCellTemplate;
            var objectArrayCellTemplate = JsonViewComparerControl.JWCObjectArrayCellTemplate;
            var objectCellTemplate = JsonViewComparerControl.JWCObjectCellTemplate;

            foreach (var item in Items)
            {
                if (item == null)
                {
                    continue;
                }

                foreach (var property in item.Items)
                {
                    if (dataTable.Columns.Contains(property.Key))
                    {
                        continue;
                    }

                    DataTemplate d = objectCellTemplate;

                    if (property is JWCProperty)
                    {
                        d = propertyCellTemplate;
                    }
                    if (property is JWCSimpleArray)
                    {
                        d = simpleArrayCellTemplate;
                    }
                    if (property is JWCObjectArray)
                    {
                        d = objectArrayCellTemplate;
                    }
                    dataTable.Columns.Add(property.Key, typeof(JWCBase));

                    var cellCollumn = new BindableDataGridColumnTemplate()
                    {
                        
                        BindingPath = $"{property.Key}",
                        CellTemplate = d,
                        Header = property.Key,
                        IsReadOnly = true
                    };

                    this.Columns.Add(cellCollumn);
                }
            }

            foreach (var item in Items)
            {
                if (item == null)
                {
                    continue;
                }

                var row = dataTable.NewRow();

                foreach (var property in item.Items)
                {
                    row[property.Key] = property;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }

    public ObservableCollection<DataGridColumn> Columns { get; set; }

    public JWCObjectArray(string key) : base(key)
    {
        this.Columns = new ObservableCollection<DataGridColumn>();
        this.Items = new List<JWCObject>();
    }

    public override void SetState(JWCState state)
    {
        base.SetState(state);

        foreach (var item in this.Items)
        {
            item?.SetState(state);
        }
    }
}