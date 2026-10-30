using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace pr5_Shitov_WPF
{
    public class ComputerBuild : INotifyPropertyChanged
    {
        private int _id;
        private string _name = "Название сборки";
        private string _purpose = "Назначение";
        private string _owner = "Владелец";

        private ObservableCollection<Component> _components = new();
        public ComputerBuild()
        {
            _components.CollectionChanged += Components_CollectionChanged;
        }

        public int Id
        {
            get => _id;
            set 
            {
                if (_id != value)
                {
                    _id = value; 
                    OnPropertyChanged(); 
                } 
            }
        }

        public string Name
        {
            get => _name;
            set 
            {
                if (_name != value) 
                {
                    _name = value; 
                    OnPropertyChanged();
                }
            }
        }

        public string Purpose
        {
            get => _purpose;
            set 
            { 
                if (_purpose != value)
                {
                    _purpose = value; 
                    OnPropertyChanged();
                }
            }
        }

        public string Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Component> Components
        {
            get => _components;
            set
            {
                if (_components != value)
                {
                    _components.CollectionChanged -= Components_CollectionChanged;
                    _components = value;
                    _components.CollectionChanged += Components_CollectionChanged;
                    OnPropertyChanged();
                }
            }
        }

        public decimal BuildTotal => Components.Sum(c => c.Price);

        private void Components_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Component c in e.NewItems)
                    c.PropertyChanged += Component_PropertyChanged;

            if (e.OldItems != null)
                foreach (Component c in e.OldItems)
                    c.PropertyChanged -= Component_PropertyChanged;

            OnPropertyChanged(nameof(BuildTotal));
        }

        private void Component_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Component.Price))
                OnPropertyChanged(nameof(BuildTotal));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
