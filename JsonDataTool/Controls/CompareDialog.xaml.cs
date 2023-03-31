using System.Windows;
using JsonDataTool.Entities;

namespace JsonDataTool.Controls
{
    /// <summary>
    /// Interaction logic for CompareDialog.xaml
    /// </summary>
    public partial class CompareDialog : Window
    {
        public JsonItem LeftJson { get;}
        public JsonItem RightJson { get; }

        public CompareDialog(JsonItem leftJson, JsonItem rightJson)
        {
            InitializeComponent();

            this.DataContext = this;
            this.LeftJson = leftJson;
            this.RightJson = rightJson;
        }
    }
}
