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
using System.Net;
using System.Diagnostics;
using System.IO.Compression;

namespace HTMLtoZip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string directoryRoot = @"..\..\Website";
        public string websiteRoot = @"..\..\Website\";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (txtAdress.Text.Trim().Length < 1)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("You did not fill in the web address entry field.", "Web address should not be empty", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!Directory.Exists(directoryRoot))
            {
                Directory.CreateDirectory(directoryRoot);
            }

            using (WebClient client = new WebClient())
            {
                try
                {
                    DateTime date = DateTime.Now;

                    string address = txtAdress.Text.Trim().ToString();
                    //Create file name
                    string htmlNameFile = websiteRoot + date.ToString($"{date.Day}_{date.Month}_{date.Year}_{date.Hour}_{date.Minute}_{date.Second}") + ".html";
                    //Download file
                    client.DownloadFile($"{address}", $"{htmlNameFile}");

                    // Or you can get the file content without saving it
                    string htmlCode = client.DownloadString($"{txtAdress.Text.Trim().ToString()}");
                    btnDownload.IsEnabled = false;
                    btnZip.IsEnabled = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("You have successfully downloaded the site", "Successfully download site", MessageBoxButton.OK);
                    btnDownload.IsEnabled = true;
                    btnZip.IsEnabled = true;
                    return;
                }
                catch (Exception)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please make sure you have entered the correct web address, please try again...", "Error download", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public static void CheckFolderEmpty(string path)
        {
            //Check if there are files in the download website folder
            if (!Directory.Exists(path))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("You did not fill in the web address entry field.", "Web address should not be empty", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //Checks for downloaded files
            if (string.IsNullOrEmpty(path))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("First Download a site.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            //Checks for downloaded .html files
            var folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                if (Directory.GetFiles(path, "*.html").Length == 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("First Download a site.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
            }
        }

        private void BtnZip_Click(object sender, RoutedEventArgs e)
        {
            string startPath = @"..\..\Website";
            string zipPath = @"..\..\ZipWebsite\ZipFile.zip";

            //Checks if there is a folder for zipped files
            if (!Directory.Exists(@"..\..\ZipWebsite"))
            {
                Directory.CreateDirectory(@"..\..\ZipWebsite");
            }
            //check if there are files in the download website folder
            if (!Directory.Exists(startPath))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("First Download a site.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            //Check if there is a folder with downloaded sites
            CheckFolderEmpty(directoryRoot);
            string zipPathCounter = "";

            try
            {
                //checks if there is a zipped file with the forwarded name
                if (File.Exists(zipPath))
                {
                    //Adds a number to the document name
                    for (int i = 1; i < 1000; i++)
                    {
                        zipPathCounter = @"..\..\ZipWebsite\ZipFile" + "(" + i + ")" + ".zip";

                        if (!File.Exists(zipPathCounter))
                        {
                            //Ziping found documents
                            ZipFile.CreateFromDirectory(startPath, zipPathCounter, CompressionLevel.Fastest, true);
                            Xceed.Wpf.Toolkit.MessageBox.Show("You have successfully zipped files.", "Successfully zip files", MessageBoxButton.OK);
                            break;
                        }
                    }
                }
                else
                {
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// opens a folder where it stores download files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            
            if (Directory.Exists(directoryRoot))
            {
                Process.Start(directoryRoot);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("The document does not currently exist", "First Download site and zipped", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        /// <summary>
        /// opens a folder where it stores zipped files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenZip_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(@"..\..\ZipWebsite"))
            {
                Process.Start(@"..\..\ZipWebsite");
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("The document does not currently exist", "First Download site and zipped", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
        }
    }
}
