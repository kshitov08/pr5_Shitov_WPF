using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    public partial class ListPage : Page
    {
        private const string Catalog = "Builds";


        public ObservableCollection<ComputerBuild> Builds { get; set; } = new();
        public ComputerBuild? SelectedBuild { get; set; }

        public ListPage()
        {
            InitializeComponent();
            Directory.CreateDirectory(Catalog);
            LoadAll();
            DataContext = this;
        }

        private void LoadAll()
        {
            if (!Directory.Exists(Catalog)) return;

            foreach (var file in Directory.GetFiles(Catalog, "*.json"))
            {
                try
                {
                    var build = JsonSerializer.Deserialize<ComputerBuild>(File.ReadAllText(file));
                    if (build != null) Builds.Add(build);
                }
                catch
                {
                    MessageBox.Show("Не удалось загрузить файл", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            var sorted = Builds.OrderBy(b => b.Id).ToList();
            Builds.Clear();
            foreach (var b in sorted) Builds.Add(b);
        }

        private int GenerateNewId()
        {
            int id = 1;
            while (File.Exists(System.IO.Path.Combine(Catalog, $"{id}.json")))
                id++;
            return id;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BuildPage(Builds, GenerateNewId()));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBuild == null)
            {
                MessageBox.Show("Не выбрана сборка.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            NavigationService.Navigate(new BuildPage(Builds, SelectedBuild));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBuild == null)
            {
                MessageBox.Show("Не выбрана сборка.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить сборку \"{SelectedBuild.Name}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            string path = System.IO.Path.Combine(Catalog, $"{SelectedBuild.Id}.json");
            if (File.Exists(path)) File.Delete(path);

            Builds.Remove(SelectedBuild);
        }
    }
}
