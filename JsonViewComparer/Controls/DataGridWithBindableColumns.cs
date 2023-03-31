using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace JsonViewComparer.Controls
{
    internal class DataGridWithBindableColumns : DataGrid
    {
        private static readonly Dictionary<DataGrid, NotifyCollectionChangedEventHandler> handlers = new();

        public ObservableCollection<DataGridColumn> BindableColumns
        {
            get => (ObservableCollection<DataGridColumn>)GetValue(BindableColumnsProperty);
            set => SetValue(BindableColumnsProperty, value);
        }

        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.Register(nameof(BindableColumns), typeof(ObservableCollection<DataGridColumn>),
                typeof(DataGridWithBindableColumns), new PropertyMetadata(null, OnBindableColumnsPropertyChanged));


        private static void OnBindableColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DataGrid dataGrid)
            {
                return;
            }

            if (e.OldValue is ObservableCollection<DataGridColumn> oldColumns)
            {
                dataGrid.Columns.Clear();

                NotifyCollectionChangedEventHandler h;

                if (handlers.TryGetValue(dataGrid, out h))
                {
                    oldColumns.CollectionChanged -= h;
                    handlers.Remove(dataGrid);
                }
            }

            var newColumns = e.NewValue as ObservableCollection<DataGridColumn>;
            dataGrid.Columns.Clear();

            if (newColumns != null)
            {
                foreach (DataGridColumn column in newColumns)
                {
                    dataGrid.Columns.Add(column);
                }

                NotifyCollectionChangedEventHandler h = (_, ne) => OnCollectionChanged(ne, dataGrid);
                handlers[dataGrid] = h;
                newColumns.CollectionChanged += h;
            }
        }

        private static void OnCollectionChanged(NotifyCollectionChangedEventArgs ne, DataGrid dataGrid)
        {
            switch (ne.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    {
                        dataGrid.Columns.Clear();

                        if (ne.NewItems != null)
                        {
                            foreach (DataGridColumn column in ne.NewItems)
                            {
                                dataGrid.Columns.Add(column);
                            }
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Add:
                    {
                        if (ne.NewItems != null)
                        {
                            foreach (DataGridColumn column in ne.NewItems)
                            {
                                dataGrid.Columns.Add(column);
                            }
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Move:
                    {
                        dataGrid.Columns.Move(ne.OldStartingIndex, ne.NewStartingIndex);

                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (DataGridColumn column in ne.OldItems)
                        {
                            dataGrid.Columns.Remove(column);
                        }

                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        dataGrid.Columns[ne.NewStartingIndex] = ne.NewItems[0] as DataGridColumn;

                        break;
                    }
            }
        }

        public void Clear()
        {
            if (handlers.TryGetValue(this, out var h))
            {
                this.BindableColumns.CollectionChanged -= h;
                handlers.Remove(this);
            }
        }

        public static void ClearAll()
        {
            foreach (var eventHandler in handlers)
            {
                if (eventHandler.Key.ItemsSource is INotifyCollectionChanged collection)
                {
                    collection.CollectionChanged -= eventHandler.Value;
                }
            }

            handlers.Clear();
        }
    }
}
