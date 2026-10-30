using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
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
    public partial class BuildPage : Page
    {
        private const string Catalog = "Builds";

        private ObservableCollection<ComputerBuild> _builds;
        private ComputerBuild _build;
        private ComputerBuild? _selectedComputerBuild;
        public Component? SelectedComponent { get; set; }
        private bool _isNew;
        public BuildPage(ObservableCollection<ComputerBuild> builds, int newId)
        {
            InitializeComponent();
            _isNew = true;
            _builds = builds;
            _build = new ComputerBuild { Id = newId};
            DataContext = _build;
        }
        public BuildPage(ObservableCollection<ComputerBuild> builds, ComputerBuild selectedComputerBuild)
        {
            InitializeComponent();
            _isNew = false;
            _builds = builds;
            _selectedComputerBuild = selectedComputerBuild;
            _build = new ComputerBuild
            {
                Id = _selectedComputerBuild.Id,
                Name = _selectedComputerBuild.Name,
                Purpose = _selectedComputerBuild.Purpose,
                Owner = _selectedComputerBuild.Owner
            };
            foreach (var c in selectedComputerBuild.Components)
                _build.Components.Add(new Component
                {
                    Name = c.Name,
                    Type = c.Type,
                    Price = c.Price
                });    
            DataContext = _build;
        }
        //Добавить
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ComponentPage(_build.Components));
        }
        //Назад
        private void Button_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        //Редактировать
        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            if (SelectedComponent == null)
            {
                MessageBox.Show("Не выбран компонент.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            NavigationService.Navigate(new ComponentPage(_build.Components, SelectedComponent));
        }
        //Сохранить
        private void Button_Save(object sender, RoutedEventArgs e)
        {
            if (_isNew == true)
            {
                _builds.Add(_build);
            }
            else 
            { 
                _selectedComputerBuild.Name = _build.Name;
                _selectedComputerBuild.Purpose = _build.Purpose;
                _selectedComputerBuild.Owner = _build.Owner;
                _selectedComputerBuild.Components.Clear();
                foreach (var c in _build.Components)
                {
                    _selectedComputerBuild.Components.Add(c);
                }
            }
            SaveToDisk();
            NavigationService.GoBack();
        }

        private void SaveToDisk()
        {
            var saved = _isNew ? _build : _selectedComputerBuild;
            var options = new JsonSerializerOptions { WriteIndented = true };
            string path = System.IO.Path.Combine(Catalog, $"{saved.Id}.json");
            File.WriteAllText(path, JsonSerializer.Serialize(saved, options));
        }
        //Удалить
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            if (SelectedComponent == null)
            {
                MessageBox.Show("Не выбран компонент.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить компонент \"{SelectedComponent.Name}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                _build.Components.Remove(SelectedComponent);
        }
    }
}
