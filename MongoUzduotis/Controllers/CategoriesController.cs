using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoUzduotis.Contracts;
using MongoUzduotis.Models;
using System;
using System.Threading.Tasks;

namespace MongoUzduotis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(new ObjectId(id));
                if (category == null)
                    return NotFound();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                await _categoryRepository.CreateAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Category category)
        {
            try
            {
                if (id != category.Id.ToString())
                    return BadRequest("ID mismatch");

                var existingCategory = await _categoryRepository.GetByIdAsync(new ObjectId(id));
                if (existingCategory == null)
                    return NotFound();

                await _categoryRepository.UpdateAsync(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(new ObjectId(id));
                if (category == null)
                    return NotFound();

                await _categoryRepository.DeleteAsync(new ObjectId(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
