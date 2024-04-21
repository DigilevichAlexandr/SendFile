using Domain;
using Infrastructure;
using Newtonsoft.Json;

namespace Application
{
    public class PictureStorage : IPictureStorage
    {
        public async Task<Picture> GetImageByDescription(string description)
        {
            string loadedJson = await File.ReadAllTextAsync("pictures.json");
            List<Picture> pictures = JsonConvert.DeserializeObject<List<Picture>>(loadedJson);
            var picture = pictures.FirstOrDefault(p=> p.Description == description);

            return picture;
        }

        public async Task<IEnumerable<Picture>> GetPictures()
        {
            string loadedJson = await System.IO.File.ReadAllTextAsync("pictures.json");
            List<Picture> pictures = JsonConvert.DeserializeObject<List<Picture>>(loadedJson);

            return pictures;
        }

        public async Task SavePicture(Picture picture)
        {
            string loadedJson = await File.ReadAllTextAsync("pictures.json");
            List<Picture> pictures = JsonConvert.DeserializeObject<List<Picture>>(loadedJson);
            pictures.Add(picture);

            string json = JsonConvert.SerializeObject(pictures);
            System.IO.File.WriteAllText("pictures.json", json);
        }
    }
}
