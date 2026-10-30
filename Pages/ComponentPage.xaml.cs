using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pr5_Shitov_WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComponentPage.xaml
    /// </summary>
    public partial class ComponentPage : Page
    {
        private ObservableCollection<Component> _components;
        private Component? _selectedComponent;
        private Component _component;
        public ComponentPage(ObservableCollection<Component> components)
        {           
            InitializeComponent();
            _components = components;
            _component = new Component();
            DataContext = _component;
        }
        public ComponentPage(ObservableCollection<Component> components, Component selectedComponent)
        {            
            InitializeComponent();
            _components = components;
            _selectedComponent = selectedComponent;
            _component = new Component
            {
                Name = _selectedComponent.Name,
                Type = _selectedComponent.Type,
                Price = _selectedComponent.Price
            };  
            DataContext = _component;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_selectedComponent == null)
            {
                _components.Add(_component);
            }
            else
            {
                _selectedComponent.Name = _component.Name;
                _selectedComponent.Type = _component.Type;
                _selectedComponent.Price = _component.Price;
            }
            NavigationService.GoBack();
        }
    }
}
