using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
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
using SharpCompress.Common;
using SharpCompress.Archives;
using System.Diagnostics;

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
                    File.Delete(path + @"\Assassins Creed Remastered Launcher.exe");
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async Task DownloadNewVersion()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                    await client.DownloadFileTaskAsync(new Uri("https://github.com/AssassinsCreedRemastered/Assassins-Creed-Remastered-Launcher/releases/download/latest/Launcher.zip"), path + @"\Launcher.zip");
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        // This is used to show progress on the ProgressBar
        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress.Value = e.ProgressPercentage;
        }

        private async Task Installation()
        {
            try
            {
                if (!Directory.Exists(path + @"\Update"))
                {
                    Directory.CreateDirectory(path + @"\Update");
                }
                await Extract(path + @"\Launcher.zip", path + @"\Update");
                await Move();
                await Cleanup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        // Used to extract files
        private async Task Extract(string fullPath, string directory)
        {
            try
            {
                using (var archive = ArchiveFactory.Open(fullPath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (!entry.IsDirectory)
                        {
                            entry.WriteToDirectory(directory, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }
                GC.Collect();
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async Task Move()
        {
            try
            {
                if (Directory.Exists(path + @"\Update"))
                {
                    foreach (string file in Directory.GetFiles(path + @"\Update"))
                    {
                        if (System.IO.Path.GetFileName(file) != "Assassins Creed Remastered Launcher Updater.exe")
                        {
                            System.IO.File.Move(file, path + @"\" + System.IO.Path.GetFileName(file), true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async Task Cleanup()
        {
            try
            {
                if (File.Exists(path + @"\Launcher.zip"))
                {
                    File.Delete(path + @"\Launcher.zip");
                }
                if (Directory.Exists(path + @"\Update"))
                {
                    Directory.Delete(path + @"\Update", true);
                }

                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetDirectory();
            DeleteOldLauncher();
            await DownloadNewVersion();
            await Installation();
            OpenLauncher.IsEnabled = true;
        }

        private void OpenLauncher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process Launcher = new Process();
                Launcher.StartInfo.WorkingDirectory = path;
                Launcher.StartInfo.FileName = "Assassins Creed Remastered Launcher.exe";
                Launcher.StartInfo.UseShellExecute = true;
                Launcher.Start();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
