using JsonDataTool.Controls;
using JsonDataTool.Entities;
using JsonDataTool.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace JsonDataTool
{
    internal class MainWindowPresenter : INotifyPropertyChanged
    {
        private bool isSelectedAll;
        private JsonItem selectedJsonItem;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<IToolbarItem> ToolbarItems { get; }

        public ObservableCollection<JsonItem> JsonItems { get; }

        public JsonItem SelectedJsonItem
        {
            get => selectedJsonItem;
            set
            {
                selectedJsonItem = value;

                if (selectedJsonItem != null)
                {
                    selectedJsonItem.IsSelected = !selectedJsonItem.IsSelected;
                }

                this.NotifyPropertyChanged();
            }
        }

        public bool IsSelectedAll
        {
            get => isSelectedAll;
            set
            {
                isSelectedAll = value;
                this.SetSelection(value);
                this.NotifyPropertyChanged();
            }
        }

        public MainWindowPresenter()
        {
            JsonItems = new();
            ToolbarItems = new()
            {
                new ToolbarItem(new Command(Reload), "ReloadIcon", "Reload", "Reload items"),
                new ToolbarItem(new Command(Save), "SaveIcon", "Save", "Save items"),
                new SeparatorToolbarItem(),
                new ToolbarItem(new Command(Add), "AddIcon", "Add", "Add new item"),
                new ToolbarItem(new Command(Remove, () => JsonItems.Any(j => j.IsSelected)),
                    "RemoveIcon", "Remove", "Remove selected items"),
                new SeparatorToolbarItem(),
                new ToolbarItem(new Command(this.Compare, () => this.JsonItems.Count(j=>j.IsSelected) == 2),
                    "CompareIcon","Compare", "Compare two selected items"),
                new ToolbarItem(new Command(this.Reformat, () => this.SelectedJsonItem != null),
                    "ReformatIcon","Reformat", "Reformat json in selected item")
            };

            this.Reload();
        }

        private void Reformat()
        {
            string jsonValue = this.SelectedJsonItem.JsonValue;

            if (string.IsNullOrEmpty(jsonValue))
            {
                return;
            }

            try
            {
                var jObject = JObject.Parse(jsonValue);

                this.selectedJsonItem.JsonValue = jObject.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Json can not be reformat.", "Error reformat json", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                DataProvider.SaveJsonData(JsonItems.ToList());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void Reload()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                foreach (var jsonItem in JsonItems)
                {
                    jsonItem.PropertyChanged -= OnJsonItemPropertyChanged;
                }

                JsonItems.Clear();

                var loadedData = DataProvider.LoadJsonData();

                foreach (var jsonItem in loadedData)
                {
                    jsonItem.PropertyChanged += OnJsonItemPropertyChanged;
                    JsonItems.Add(jsonItem);
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void RefreshCommands()
        { 
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var toolbarItem in ToolbarItems.OfType<ToolbarItem>())
                {
                    toolbarItem.Refresh();
                }
            });
        }

        private void Add()
        {
            var entityToAdd = new JsonItem();
            entityToAdd.PropertyChanged += OnJsonItemPropertyChanged;
            this.JsonItems.Add(entityToAdd);
            this.SelectedJsonItem = entityToAdd;
        }
        
        private void OnJsonItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(JsonItem.IsSelected))
            {
                this.RefreshCommands();
            }
        }

        private void Compare()
        {
            var entitiesToCompare = this.JsonItems.Where(x => x.IsSelected).ToList();

            if (entitiesToCompare.Count != 2)
            {
                return; 
            }

            var compareDialog = new CompareDialog(entitiesToCompare[0], entitiesToCompare[1]);

            compareDialog.Show();
        }

        private void Remove()
        {
            var entitiesToRemove = this.JsonItems.Where(j => j.IsSelected).ToList();

            foreach (var entityToRemove in entitiesToRemove)
            {
                entityToRemove.PropertyChanged -= OnJsonItemPropertyChanged;
                this.JsonItems.Remove(entityToRemove);
            }

            this.RefreshCommands();
        }

        private void SetSelection(bool selected)
        {
            foreach (var jsonItem in this.JsonItems)
            {
                jsonItem.IsSelected = selected;
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
