using AutoMapper;
using Cookbook.Controllers.Api;
using Cookbook.Data;
using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Xunit;


namespace Cookbook.Tests
{
    public class ApiRecipeControllerTest
    {
        //private readonly IServiceProvider serviceProvider;
        private ILogger<RecipeController> _logger = new LoggerFactory().AddDebug().CreateLogger<RecipeController>();
        private string _username = "test@test.com";
        private readonly Recipe _sampleRecipe = new Recipe
        {
            Name = "Lemon Drizzle",
            Serves = 8,
            UserName = "test@test.com",
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
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiRecipeControllerTest()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<RecipeViewModel, Recipe>().ReverseMap();
                config.CreateMap<IngredientViewModel, Ingredient>().ReverseMap();
                config.CreateMap<InstructionViewModel, Instruction>().ReverseMap();
            });
        }

        [Fact(DisplayName = "Get all success")]
        public void get_success_all()
        {
            // Arrange
            using (var dbContext = new CookbookContext(CreateNewContextOptions()))
            {
                CreateTestData(dbContext, _sampleRecipe);
                var repo = new CookbookRepo(dbContext);
                var controller = new RecipeControllerTest(repo, _logger, _username);

                // Act
                var result = controller.Get() as ObjectResult;

                // Assert
                Assert.IsAssignableFrom(typeof(ObjectResult), result);
                Assert.IsType(typeof(List<Recipe>), result.Value);
            }
        }

        [Theory(DisplayName = "Get by Id")]
        [InlineData(1, 200)]
        [InlineData(999, 400)]
        public void get_byId(int id, int expectedStatusCode)
        {
            // Arrange
            using (var dbContext = new CookbookContext(CreateNewContextOptions()))
            {
                CreateTestData(dbContext, _sampleRecipe);
                var repo = new CookbookRepo(dbContext);
                var controller = new RecipeController(repo, _logger);

                // Act
                var result = controller.Get(id) as ObjectResult;

                // Assert
                Assert.IsAssignableFrom(typeof(ObjectResult), result);
                Assert.Equal(expectedStatusCode, result.StatusCode);
            }
        }

        [Fact(DisplayName = "Post recipe successful")]
        public async void Post_recipe_successful()
        {
            // Arrange
            using (var dbContext = new CookbookContext(CreateNewContextOptions()))
            {
                CreateTestData(dbContext);
                var repo = new CookbookRepo(dbContext);
                var controller = new RecipeControllerTest(repo, _logger, _username);

                // Act
                var result = await controller.Post(Mapper.Map<RecipeViewModel>(_sampleRecipe)) as ObjectResult;

                //Assert
                Assert.IsAssignableFrom(typeof(ObjectResult), result);
                Assert.Equal(201, result.StatusCode);
                Assert.IsType(typeof(CreatedResult), (CreatedResult)result);
                CreatedResult createdResult = (CreatedResult)result;
                Assert.Equal("api/recipe/1", createdResult.Location);
            }
        }

        [Fact(DisplayName = "Post a recipe failed")]
        public async void Post_recipe_failed()
        {
            // Arrange
            using (var dbContext = new CookbookContext(CreateNewContextOptions()))
            {
                CreateTestData(dbContext, _sampleRecipe);
                var repo = new CookbookRepo(dbContext);
                var controller = new RecipeController(repo, _logger);

                // Act
                var result = await controller.Post(new RecipeViewModel { Id = 1 }) as ObjectResult;

                //Assert
                Assert.IsAssignableFrom(typeof(ObjectResult), result);
                Assert.Equal(400, result.StatusCode);
            }
        }

        [Theory(DisplayName = "Edit a recipe")]
        [InlineData(1, "Sponge Cake", 204)]
        [InlineData(999, "Sponge Cake", 404)]
        public async void Put_a_recipe(int id, string expectedName, int expectedStatusCode)
        {
            // Arrange
            using (var dbContext = new CookbookContext(CreateNewContextOptions()))
            {
                CreateTestData(dbContext, _sampleRecipe);
                var repo = new CookbookRepo(dbContext);
                var controller = new RecipeControllerTest(repo, _logger, _username);
                var editRecipe = new Recipe();
                editRecipe.Id = id;
                editRecipe.Name = expectedName;
                editRecipe.Serves = _sampleRecipe.Serves;
                editRecipe.Ingredients = _sampleRecipe.Ingredients;
                editRecipe.Method = _sampleRecipe.Method;

                // Act
                var result = await controller.Put(id, Mapper.Map<RecipeViewModel>(editRecipe)) as StatusCodeResult;

                //Assert
                Assert.IsAssignableFrom(typeof(StatusCodeResult), result);
                Assert.Equal(expectedStatusCode, result.StatusCode);
            }
        }

        [Theory(DisplayName = "Delete a recipe")]
        [InlineData(1, 204)]
        [InlineData(999, 404)]
        public async void Delete_a_recipe(int id, int expectedStatusCode)
        {
            // Arrange
            using (var dbContext = new CookbookContext(CreateNewContextOptions()))
            {
                CreateTestData(dbContext, _sampleRecipe);
                var repo = new CookbookRepo(dbContext);
                var controller = new RecipeController(repo, _logger);

                // Act
                var result = await controller.Delete(id) as StatusCodeResult;

                //Assert
                Assert.IsAssignableFrom(typeof(StatusCodeResult), result);
                Assert.Equal(expectedStatusCode, result.StatusCode);
            }
        }

        private void CreateTestData(CookbookContext dbContext, params Recipe[] recipes)
        {
            dbContext.Recipes.AddRange(recipes);
            dbContext.SaveChanges();
        }

        private static DbContextOptions<CookbookContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new unique service provider.
            var builder = new DbContextOptionsBuilder<CookbookContext>();
            builder.UseInMemoryDatabase($"database{Guid.NewGuid()}")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }

    /// <summary>
    /// Recipe test controller overridden due to authentication
    /// </summary>
    public class RecipeControllerTest : RecipeController
    {
        private string _userName;

        public RecipeControllerTest(ICookbookRepo repo, ILogger<RecipeController> logger, string userName) 
            : base(repo, logger)
        {
            _userName = userName;
        }

        public override string GetUserIdentityName()
        {
            return _userName;
        }
    }
}
