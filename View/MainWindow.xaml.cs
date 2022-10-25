using FileManager.Models;
using FileManager.View;
using FileManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            MainViewModel vm = (MainViewModel)DataContext;
            vm.CurrentPath = ((IModel)item.Content).Path;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileVisitHistoryWindow fileVisitHistoryWindow = new FileVisitHistoryWindow();
            fileVisitHistoryWindow.ShowDialog();
        }
    }
}
