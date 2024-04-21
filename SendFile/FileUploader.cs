using Domain;
using System;
using System.IO;
using System.Net.Http;

namespace SendFile
{
    public class FileUploader
    {
        public async Task<string> UploadFile(Picture picture)
        {
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                // Добавляем текстовое содержимое
                formData.Add(new StringContent(picture.Description), "description");

                // Добавляем файл
                byte[] fileBytes = File.ReadAllBytes(picture.FilePath);
                formData.Add(new ByteArrayContent(fileBytes), "file", Path.GetFileName(picture.FilePath));

                // Отправляем запрос на сервер
                var response = await client.PostAsync("https://localhost:7070/api/File", formData);

                // Обработка ответа
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Server returned {response.StatusCode}: {response.ReasonPhrase}");
                }
            }
        }
    }
}
