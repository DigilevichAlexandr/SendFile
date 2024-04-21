using Domain;

namespace Infrastructure
{
    public interface IPictureStorage
    {
        Task SavePicture(Picture picture);
        Task<IEnumerable<Picture>> GetPictures();
        Task<Picture> GetImageByDescription(string description);
    }
}
