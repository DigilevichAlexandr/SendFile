using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace FileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        IPictureStorage _pictureStorage;

        public FileController(IPictureStorage pictureStorage)
        {
            _pictureStorage = pictureStorage;
        }

        [HttpGet]
        public async Task<IEnumerable<Picture>> GetPictures()
        {
            var pictures = await _pictureStorage.GetPictures();

            return pictures.Select(p => new Picture() { Description = p.Description}); // чтоб пути к файлам на сервере не попали на клиет
        }

        [HttpGet("{imageDescription}")]
        public async Task<IActionResult> GetImage(string imageDescription)
        {
            var picture = await _pictureStorage.GetImageByDescription(imageDescription);

            if(picture == null) { return NotFound(); }

            // Проверяем, существует ли файл
            if (!System.IO.File.Exists(picture.FilePath))
            {
                return NotFound(); // Возвращаем 404 Not Found, если файл не найден
            }

            // Загружаем файл и отправляем его клиенту
            try
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(picture.FilePath);
                return File(imageBytes, "image/jpeg"); // Отправляем изображение клиенту
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                return StatusCode(500, ex.Message); // Возвращаем 500 Internal Server Error в случае ошибки
            }
        }

        [HttpPost]
        public async Task<ActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            var description = Request.Form["description"];

            // путь к папке, где будут храниться файлы
            var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
            // создаем папку для хранения файлов
            Directory.CreateDirectory(uploadPath);

            // путь к папке uploads
            string fullPath = $"{uploadPath}/{file.FileName}";

            // сохраняем файл в папку uploads
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var picture = new Picture() { Description = description, FilePath = fullPath };

            await _pictureStorage.SavePicture(picture);

            return Ok(new { Result = "Файлы успешно загружены" });
        }
    }
}
