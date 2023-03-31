using JsonDataTool.Controls;

namespace JsonDataTool.Entities
{
    public class ToolbarItem : Entity, IToolbarItem
    {
        public Command Command { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Tooltip { get; set; }


        public ToolbarItem(Command command, string icon,  string text, string tooltip)
        {
            Command = command;
            Text = text;
            Icon = icon;
            Tooltip = tooltip;
        }

        public void Refresh()
        {
            this.Command?.RaiseCanExecuteChanged();
        }
    }
}
