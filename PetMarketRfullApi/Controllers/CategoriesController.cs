using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Services;

namespace PetMarketRfullApi.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : Controller
    {
        //Внедрение зависимости
        private readonly ICategoryServices _categoryService;

        public CategoriesController(ICategoryServices categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return View();
        }
    }
}
