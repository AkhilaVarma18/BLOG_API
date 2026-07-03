using Blog.API.Models.Domain;

namespace Blog.API.Reposittory.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAll();

    }
}
