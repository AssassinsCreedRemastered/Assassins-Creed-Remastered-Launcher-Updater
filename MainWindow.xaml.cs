using System;
using System.Collections.Generic;
using System.IO;
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

namespace Assassins_Creed_Remastered_Launcher_Updater
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

        // Global
        private string path = "";

        // Functions
        // Grabbing path where AC is installed
        private async void GetDirectory()
        {
            try
            {
                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ubisoft\Assassin's Creed\Path.txt"))
                {
                    path = sr.ReadLine();
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async void DeleteOldLauncher()
        {
            try
            {
                if (File.Exists(path + @"\Assassins Creed Remastered Launcher.exe"))
                {
                    Console.WriteLine("Exists");
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetDirectory();
            DeleteOldLauncher();
        }
    }
}
