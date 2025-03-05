﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : Controller
    {
        //Внедрение зависимости
        private readonly ICategoryServices _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryServices categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResource>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoriesResources = _mapper.Map<IEnumerable<CategoryResource>>(categories);
            return Ok(categoriesResources);
        }

        //GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResource>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryResource = _mapper.Map<CategoryResource>(category);
            return Ok(categoryResource);
        }

        //POST: api/categories/post
        [HttpPost]
        public async Task<ActionResult<CategoryResource>> CreateCategory(CreateCategoryResource createCategoryResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryResource = await _categoryService.CreateCategoryAsync(createCategoryResource);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryResource.Id }, categoryResource);
        }

        //DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryResource>> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message); // 404 not found
            }
        }
    }
}
