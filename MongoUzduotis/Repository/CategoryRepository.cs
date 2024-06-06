﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoUzduotis.Contracts;
using MongoUzduotis.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoUzduotis.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("yourDatabaseName");
            _categories = database.GetCollection<Category>("categories");
        }

        public async Task CreateAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _categories.DeleteOneAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(ObjectId id)
        {
            return await _categories.Find<Category>(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            await _categories.ReplaceOneAsync(c => c.Id == category.Id, category);
        }
    }
}
