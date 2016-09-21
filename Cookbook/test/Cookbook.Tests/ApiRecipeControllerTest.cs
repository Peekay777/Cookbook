using AutoMapper;
using Cookbook.Controllers.Api;
using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cookbook.Tests
{
    public class ApiRecipeControllerTest
    {
        private CookbookContext _context;
        private CookbookRepo _repo;
        private RecipeController _controller;
        private Recipe _sampleRecipe = new Recipe
        {
            Name = "Lemon Drizzle",
            Serves = 8,
            Ingredients = new List<Ingredient>
                {
                    new Ingredient { Description="200g Demerara Sugar" },
                    new Ingredient { Description="200g Butter" },
                    new Ingredient { Description="400g Flour" },
                    new Ingredient { Description="1 Lemon" },
                    new Ingredient { Description="2 Eggs" },
                    new Ingredient { Description="200g Caster Sugar" }
                },
            Method = new List<Instruction>
                {
                    new Instruction { Task="Heat the butter and mix in demerara sugar until melted" },
                    new Instruction { Task="Add sifted flour and the eggs and mix until smooth" },
                    new Instruction { Task="Put into pre-heated oven for 35-40 minutes until golden brown" }
                }
        };

        public ApiRecipeControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<RecipeViewModel, Recipe>().ReverseMap();
                config.CreateMap<IngredientViewModel, Ingredient>().ReverseMap();
                config.CreateMap<InstructionViewModel, Instruction>().ReverseMap();
            });
        }

        [Fact(DisplayName = "Get all")]
        public void Get_all()
        {
            // Arrange
            Setup(_sampleRecipe);

            // Act
            var result = _controller.Get() as ObjectResult;

            // Assert
            Assert.IsType(typeof(List<Recipe>), result.Value);
            List<Recipe> model = (List<Recipe>)result.Value;
            Assert.Equal(1, model.Count);
            Assert.Equal("Lemon Drizzle", model[0].Name);
            Assert.Equal(8, model[0].Serves);
            Assert.Equal(6, model[0].Ingredients.Count);
            Assert.Equal(3, model[0].Method.Count);
        }

        [Theory(DisplayName = "Get Recipe by Id")]
        [InlineData(999, 400, typeof(RecipeViewModel), "", 0)]
        [InlineData(1, 200, typeof(RecipeViewModel), "Lemon Drizzle", 8)]
        public void Get_recipe_by_id(int id, int expectedStatusCode, Type expectedType, string expectedName, int expectedServes)
        {
            // Arrange
            Setup(_sampleRecipe);

            // Act
            var result = _controller.Get(id) as ObjectResult;

            // Assert
            Assert.Equal(expectedStatusCode, result.StatusCode);
            if (expectedStatusCode == 200)
            {
                Assert.IsType(expectedType, result.Value);
                RecipeViewModel model = (RecipeViewModel)result.Value;
                Assert.Equal(expectedName, model.Name);
                Assert.Equal(expectedServes, model.Serves);
            }
        }

        [Fact(DisplayName = "Post recipe successful")]
        public async void Post_recipe_successful()
        {
            // Arrange
            Setup();

            // Act
            var result = await _controller.Post(Mapper.Map<RecipeViewModel>(_sampleRecipe)) as ObjectResult;

            //Assert
            Assert.IsAssignableFrom(typeof(ObjectResult), result);
            Assert.Equal(201, result.StatusCode);
            Assert.IsType(typeof(CreatedResult), (CreatedResult)result);
            CreatedResult createdResult = (CreatedResult)result;
            Assert.Equal("api/recipe/1", createdResult.Location);
        }

        [Fact(DisplayName = "Post a recipe failed")]
        public async void Post_recipe_failed()
        {
            // Arrange
            Setup(_sampleRecipe);

            // Act
            var result = await _controller.Post(new RecipeViewModel { Id = 1 }) as ObjectResult;

            //Assert
            Assert.IsAssignableFrom(typeof(ObjectResult), result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType(typeof(BadRequestObjectResult), result);
        }

        [Theory(DisplayName = "Delete a recipe success")]
        [InlineData(1)]
        [InlineData(999)]
        public async void Delete_a_recipe(int id)
        {
            // Arrange
            Setup(_sampleRecipe);
            // Act
            var result = await _controller.Delete(id) as StatusCodeResult;

            // Assert
            Assert.IsAssignableFrom(typeof(StatusCodeResult), result);
            if (result.GetType() == typeof(NoContentResult))
            {
                Assert.Equal(204, result.StatusCode);
            }
            else
            {
                Assert.Equal(404, result.StatusCode);
            }
        }

        /// <summary>
        /// Refresh the db context with new data
        /// </summary>
        /// <param name="recipes"></param>
        private void Setup(params Recipe[] recipes)
        {
            // Initialize DbContext in memory
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();

            _context = new CookbookContext(optionsBuilder.Options);

            // Seed data
            foreach (var recipe in recipes)
            {
                _context.Add(recipe);
            }
            _context.SaveChanges();

            // create repo
            _repo = new CookbookRepo(_context);

            // Create test subject
            _controller = new RecipeController(_repo);
        }
    }
}
