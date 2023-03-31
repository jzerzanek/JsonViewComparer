using JsonDataTool.Entities;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace JsonDataTool.Controls
{
    /// <summary>
    /// Interaction logic for ToolbarItems.xaml
    /// </summary>
    public partial class ToolbarItemsControl : UserControl
    {
        public IEnumerable<IToolbarItem> Items
        {
            get => (IEnumerable<IToolbarItem>) GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(IEnumerable<IToolbarItem>), typeof(ToolbarItemsControl));

        public ToolbarItemsControl()
        {
            InitializeComponent();
        }
    }
}
