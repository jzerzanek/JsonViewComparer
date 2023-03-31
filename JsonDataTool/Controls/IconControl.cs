using System.Windows;
using System.Windows.Controls;

namespace JsonDataTool.Controls
{
    public class IconControl : Control
    {
        public ControlTemplate Source
        {
            get => (ControlTemplate)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ControlTemplate), typeof(IconControl), new PropertyMetadata(default(ControlTemplate), SourceChanged));

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (IconControl)d;

            control.Template = (ControlTemplate)e.NewValue;
        }
    }
}
