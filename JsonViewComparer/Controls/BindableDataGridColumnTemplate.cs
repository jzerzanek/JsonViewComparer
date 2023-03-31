using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace JsonViewComparer.Controls
{
    internal class BindableDataGridColumnTemplate : DataGridTemplateColumn
    {
        public string BindingPath { get; set; }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            switch (cell.DataContext)
            {
                case DataRowView r:
                {
                    if (!string.IsNullOrEmpty(this.BindingPath))
                    {
                        cell.DataContext = r.Row[this.BindingPath];
                    }
                    else
                    {
                        cell.DataContext = r.Row.ItemArray[cell.Column.DisplayIndex];
                    }

                    break;
                }
            }

            return base.GenerateElement(cell, dataItem);
        }
    }
}
