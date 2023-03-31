using System;

namespace JsonDataTool.Entities
{
    public class JsonItem : Entity
    {
        private const int MaxFormatJsonValueLength = 100;

        private bool isSelected;
        private string jsonValue;
        private string name;

        public Guid Id { get; set; }

        public string Name
        {
            get => name;
            set => this.ExecuteAndNotify(() => name = value);
        }

        public bool IsSelected
        {
            get => isSelected;
            set => this.ExecuteAndNotify(() => isSelected = value);
        }

        public string JsonValue
        {
            get => jsonValue;
            set => ExecuteAndNotify(() =>
            {
                jsonValue = value;
                NotifyPropertyChanged(nameof(FormatedJsonValue));
            });
        }

        public string FormatedJsonValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.JsonValue))
                {
                    return this.JsonValue;
                }

                if (this.JsonValue.Length <= MaxFormatJsonValueLength)
                {
                    return this.JsonValue;
                }

                return $"{JsonValue.Substring(0, MaxFormatJsonValueLength)}...";
            }
        }

        public JsonItem()
        {
            Id = Guid.NewGuid();
        }
    }
}
