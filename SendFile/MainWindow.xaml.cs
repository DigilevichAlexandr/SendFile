using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows;
using Domain;
using Microsoft.Win32;

namespace SendFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel();
        }

        private async void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                //ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                //string sep = string.Empty;

                //foreach (var c in codecs)
                //{
                //    string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                //    openFileDialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", openFileDialog.Filter, sep, codecName, c.FilenameExtension);
                //    sep = "|";
                //}

                openFileDialog.Filter = "Image Files|*.jpg";//String.Format("{0}{1}{2} ({3})|{3}", openFileDialog.Filter, sep, "All Files", "*.*");
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == true)
                {
                    var uploader = new FileUploader();
                    var picture = ((ApplicationViewModel)DataContext).UploadPicture;

                    if(picture == null || picture.Description == null) {
                        picture = new Picture() { Description = DateTime.Now.ToString() };
                    }

                    picture.FilePath = openFileDialog.FileName;
                    await uploader.UploadFile(picture);
                    ((ApplicationViewModel)DataContext).Pictures.Add(picture);
                }
            }
            catch (Exception ex) { }
        }
    }
}