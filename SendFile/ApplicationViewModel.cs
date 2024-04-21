using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace SendFile
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Picture selectedPicture;
        private Picture uploadPicture = new Picture();
        private string selectedPictureLocalPath = "";

        public ObservableCollection<Picture> Pictures { get; set; }
        public Picture SelectedPicture
        {
            get { return selectedPicture; }
            set
            {
                // URL изображения для загрузки
                string imageUrl = $"https://localhost:7070/api/File/{value.Description}";

                // Путь для сохранения изображения
                
                var imagePath = $"{Directory.GetCurrentDirectory()}\\{value.Description}.jpg";

                // Создание экземпляра WebClient
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        // Загрузка изображения
                        client.DownloadFile(imageUrl, imagePath);

                        Console.WriteLine("Изображение загружено успешно.");

                        SelectedPictureLocalPath = imagePath;
                    }
                    catch (WebException ex)
                    {
                        // Обработка ошибок
                        Console.WriteLine("Ошибка: " + ex.Message);
                    }
                }

                selectedPicture = value;
                OnPropertyChanged("SelectedPicture");
            }
        }

        public string SelectedPictureLocalPath
        {
            get { return selectedPictureLocalPath; }
            set
            {
                selectedPictureLocalPath = value;
                OnPropertyChanged("SelectedPictureLocalPath");
            }
        }

        public Picture UploadPicture
        {
            get { return uploadPicture; }
            set
            {
                uploadPicture = value;
                OnPropertyChanged("UploadPicture");
            }
        }

        public ApplicationViewModel()
        {
            // Создание экземпляра WebClient
            using (WebClient client = new WebClient())
            {
                try
                {
                    // Выполнение GET-запроса
                    string response = client.DownloadString("https://localhost:7070/api/File");
                    List<Picture> pictures = JsonConvert.DeserializeObject<List<Picture>>(response);
                    Pictures = new ObservableCollection<Picture>(pictures);
                }
                catch (WebException ex)
                {
                    // Обработка ошибок
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
