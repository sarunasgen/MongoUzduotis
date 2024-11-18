using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoUzduotis.Contracts;
using MongoUzduotis.Models;
using Serilog;
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
                Log.Information("Infomration retrieved successfully from GetAll endpoint");
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
                {
                    Log.Warning($"No information found with {id}");
                    return NotFound();
                }
                    
                return Ok(category);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to get by id");
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

        [HttpDelete("DeleteByName/{categoryName}")]
        public async Task<IActionResult> DeleteByName(string categoryName)
        {
            IActionResult result = null;
            try
            {
                Task deleteTask = _categoryRepository.DeleteAsync(new ObjectId());
                var kategorijos = await _categoryRepository.GetAllAsync();
                foreach(var category in kategorijos)
                {
                    if(category.Name == categoryName)
                    {
                        deleteTask = _categoryRepository.DeleteAsync(category.Id);
                    }
                }
                await Task.WhenAll(deleteTask).ContinueWith(x =>
                {
                    if (x.IsCompletedSuccessfully || !x.IsFaulted)
                        result = Ok($"Category deleted {categoryName}");
                    else
                        result = NotFound($"Category {categoryName} was not deleted, because it was not found");
                });
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
