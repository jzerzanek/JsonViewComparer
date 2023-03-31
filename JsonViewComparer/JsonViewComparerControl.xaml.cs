using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JsonViewComparer.Entities;
using JsonViewComparer.Utils;

namespace JsonViewComparer
{
    /// <summary>
    /// Interaction logic for JsonViewComparerControl.xaml
    /// </summary>
    public partial class JsonViewComparerControl : UserControl, INotifyPropertyChanged
    {
        private bool isLeftJson;
        private bool isRightJson;
        private IJWCBase leftProperties;
        private IJWCBase rightProperties;

        public event PropertyChangedEventHandler PropertyChanged;

        internal static DataTemplate JWCPropertyCellTemplate;
        internal static DataTemplate JWCObjectCellTemplate;
        internal static DataTemplate JWCSimpleArrayCellTemplate;
        internal static DataTemplate JWCObjectArrayCellTemplate;

        public string LeftTitle
        {
            get => (string)GetValue(LeftTitleProperty);
            set => SetValue(LeftTitleProperty, value);
        }

        public string LeftValue
        {
            get => (string)GetValue(LeftValueProperty);
            set => this.SetValue(LeftValueProperty, value);
        }

        public bool IsLeftJson
        {
            get => this.isLeftJson;
            private set
            {
                this.isLeftJson = value;
                this.NotifyPropertyChanged();
            }
        }
        
        public IJWCBase LeftProperties
        {
            get => this.leftProperties;
            private set
            {
                this.leftProperties = value;
                this.NotifyPropertyChanged();
            }
        }

        public static readonly DependencyProperty LeftTitleProperty =
            DependencyProperty.Register(nameof(LeftTitle), typeof(string), typeof(JsonViewComparerControl),
                new PropertyMetadata("Left value"));

        public static readonly DependencyProperty LeftValueProperty =
            DependencyProperty.Register(nameof(LeftValue), typeof(string), typeof(JsonViewComparerControl),
                new PropertyMetadata(null, OnLeftPropertyChanged));

        public string RightTitle
        {
            get => (string)GetValue(RightTitleProperty);
            set => SetValue(RightTitleProperty, value);
        }

        public string RightValue
        {
            get => (string)GetValue(RightValueProperty);
            set => SetValue(RightValueProperty, value);
        }

        public bool IsRightJson
        {
            get => this.isRightJson;
            private set
            {
                this.isRightJson = value;
                this.NotifyPropertyChanged();
            }
        }

        public IJWCBase RightProperties
        {
            get => this.rightProperties;
            private set
            {
                this.rightProperties = value;
                this.NotifyPropertyChanged();
            }
        }

        public static readonly DependencyProperty RightTitleProperty =
            DependencyProperty.Register(nameof(RightTitle), typeof(string), typeof(JsonViewComparerControl),
                new PropertyMetadata("Right value"));

        public static readonly DependencyProperty RightValueProperty =
            DependencyProperty.Register(nameof(RightValue), typeof(string), typeof(JsonViewComparerControl),
                new PropertyMetadata(null, OnRightPropertyChanged));

        public HashSet<string> NoSplitKeys { get; }

        public JsonViewComparerControl()
        {
            InitializeComponent();

            this.NoSplitKeys = new HashSet<string>(); //TODO
            JWCPropertyCellTemplate = this.FindResource("JWCPropertyCellTemplate") as DataTemplate;
            JWCObjectCellTemplate = this.FindResource("JWCObjectCellTemplate") as DataTemplate;
            JWCSimpleArrayCellTemplate = this.FindResource("JWCSimpleArrayCellTemplate") as DataTemplate;
            JWCObjectArrayCellTemplate = this.FindResource("JWCObjectArrayCellTemplate") as DataTemplate;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void OnLeftPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not JsonViewComparerControl control)
            {
                return;
            }

            if (string.IsNullOrEmpty(control.LeftValue) || !control.LeftValue.TrimStart().Contains("{"))
            {
                control.IsLeftJson = false;
                control.LeftProperties = null;

                return;
            }

            try
            {
                control.IsLeftJson = true;
                control.LeftProperties = JWCHelper.GenerateProperties(control.LeftValue, control.NoSplitKeys);

                JWCHelper.CompareProperties((JWCBase)control.LeftProperties, (JWCBase)control.RightProperties, true);
                JWCHelper.CompareProperties((JWCBase)control.RightProperties, (JWCBase)control.LeftProperties, false);
            }
            catch
            {
                control.IsLeftJson = false;
                control.LeftProperties = null;
            }
        }

        private static void OnRightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not JsonViewComparerControl control)
            {
                return;
            }

            if (string.IsNullOrEmpty(control.RightValue) || !control.RightValue.TrimStart().Contains("{"))
            {
                control.IsRightJson = false;
                control.RightProperties = null;

                return;
            }

            try
            {
                control.IsRightJson = true;
                control.RightProperties = JWCHelper.GenerateProperties(control.RightValue, control.NoSplitKeys);

                JWCHelper.CompareProperties((JWCBase)control.RightProperties, (JWCBase)control.LeftProperties, false);
                JWCHelper.CompareProperties((JWCBase)control.LeftProperties, (JWCBase)control.RightProperties, true);
            }
            catch
            {
                control.IsRightJson = false;
                control.RightProperties = null;
            }
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };

                this.JsonViewComparerControlRoot?.RaiseEvent(eventArg);
            }
        }
    }
}
