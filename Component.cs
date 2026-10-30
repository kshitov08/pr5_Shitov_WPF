using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pr5_Shitov_WPF
{
    public class Component : INotifyPropertyChanged
    {
        private string _name = "Комплектующая";
        private string _type = "Тип";
        private decimal _price = 0m;

        public string Name
        {
            get => _name;
            set { if (_name != value) { _name = value; OnPropertyChanged(); } }
        }

        public string Type
        {
            get => _type;
            set { if (_type != value) { _type = value; OnPropertyChanged(); } }
        }

        public decimal Price
        {
            get => _price;
            set { if (_price != value) { _price = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}
