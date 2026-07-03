
using Blog.API.Data;
using Blog.API.Models.Domain;
using Blog.API.Models.DTO;
using Blog.API.Reposittory.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        //private readonly ApplicationDbContext dbContext;

        //public CategoriesController(ApplicationDbContext dbContext)
        //{
        //    this.dbContext = dbContext;
        //}
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category
            {

                Name = request.Name,

                UrlHandle = request.UrlHandle

            };

            await categoryRepository.CreateAsync(category);

            var response = new CategoryDto            {

            Id = category.Id,

                Name = category.Name,

                UrlHandle = category.UrlHandle

            };


            return Ok(response);
    }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            var response = new List<CategoryDto>();

            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)

        {

            var existingCategory = await categoryRepository.GetById(id);

            if (existingCategory is null)

            {

                return NotFound();

            }


            var response = new CategoryDto
            {

                Id = existingCategory.Id,

                Name = existingCategory.Name,

                UrlHandle = existingCategory.UrlHandle

            };

            return Ok(response);

        }


        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            category = await categoryRepository.UpdateAsync(category);

            if (category is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

    }
}
