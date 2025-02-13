using Bulky.Models.Models;
using BulkyWeb_Api.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWebApi.Test.UnitTesting
{
    public class ProductControllerApiTests 
    {
        private readonly InMemoryDbContext dbContext;
        public ProductControllerApiTests()
        {
           dbContext = new InMemoryDbContext();
            List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "test1", Brand = "test1", ListPrice = 50, Price1 = 85, Price50 = 750, Price100 = 2355, CategoryId = 1, Description ="test1" },
            new Product { Id = 2, Name = "test2", Brand = "test2", ListPrice = 55, Price1 = 90, Price50 = 800, Price100 = 2500, CategoryId = 2, Description = "test2" },
            new Product { Id = 3, Name = "test3", Brand = "test3", ListPrice = 45, Price1 = 75, Price50 = 750, Price100 = 2300, CategoryId = 1, Description = "test3" }
         };
            dbContext.AddRange(Products);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetProducts_ReturnsAListOfProductsFromDatabase()
        {
            // Arrange
            
            var sut = new ProductsController(dbContext);
           
            // Act
            var result = sut.GetProducts();

            // Assert
            var actionResult = Assert.IsType<Task<ActionResult<IEnumerable<Product>>>>(result);
            var productsResult = actionResult.Result;
            Assert.IsType<List<Product>>(productsResult.Value);
            var productsValue = productsResult.Value.ToList(); 
            Assert.Equal(3,productsValue.Count);

        }

        [Fact]
        public async Task GetProductById_ProductIsNull_ReturnsNotFound()
        {
            // Arrange
           
            var sut = new ProductsController(dbContext);
            // Act
            var result = sut.GetProduct(5);
            // Assert
            var actionResult = Assert.IsType<Task<ActionResult<Product>>>(result);
            var productResult = actionResult.Result;
            Assert.IsType<NotFoundResult>(productResult.Result);
        }

         [Fact]
        public async Task GetProductById_ProductExists_ReturnsProduct()
        {
            // Arrange
           
            var sut = new ProductsController(dbContext);
            // Act
            var result = sut.GetProduct(1);
            // Assert
            var actionResult = Assert.IsType<Task<ActionResult<Product>>>(result);
            var productResult = actionResult.Result;
            var productValue = productResult.Value;
            Assert.Contains("test1", productValue.Name);
        }

         [Fact]
        public async Task PostProduct_AddProduct_ReturnsProduct()
        {
            // Arrange
           
            var sut = new ProductsController(dbContext);

            // Act
            var product = new Product 
            { Id = 4, Name = "test4", Brand = "test4", ListPrice = 45, Price1 = 75, Price50 = 750, Price100 = 2300, CategoryId = 1, Description = "test4" };
           
            var result = await sut.PostProduct(product);
            var products = await sut.GetProducts();
            var AllProductsValue = products.Value.ToList();
            // Assert

            var productValue = result.Value;
            Assert.Equal(4, AllProductsValue.Count);
            Assert.Contains("test4", productValue.Name);
        }
        [Fact]
        public async Task PutProduct_EditProductTheGivenIdIsNotTheProductId_ReturnsBadRequest()
        {
            // Arrange

            var sut = new ProductsController(dbContext);
            var product =
                new Product
                {
                    Id = 10,
                    Name = "test1Updated",
                    Brand = "test1Updated",
                    ListPrice = 50,
                    Price1 = 85,
                    Price50 = 750,
                    Price100 = 2355,
                    Description = "test1Updated"
                };
            // Act
            var result = await sut.PutProduct(5, product);
            var actionResult = result.Result;
            // Assert


            Assert.IsType<BadRequestResult>(actionResult);
        }


        [Fact]
        public async Task PutProduct_EditProductNotExists_ReturnsNotFound()
        {
            // Arrange

            var sut = new ProductsController(dbContext);
            var product = 
                new Product { Id = 10, Name = "test1Updated", Brand = "test1Updated", ListPrice = 50,
                    Price1 = 85, Price50 = 750, Price100 = 2355,  Description = "test1Updated" };
            // Act
            var result = await sut.PutProduct(10,product);
            var actionResult =result.Result;
            // Assert


            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task PutProduct_EditProductWithId_ReturnsUpdated()
        {
            // Arrange

            var sut = new ProductsController(dbContext);
            var product =
                new Product
                {
                    Id = 1,
                    Name = "test1Updated",
                    Brand = "test1Updated",
                    ListPrice = 50,
                    Price1 = 85,
                    Price50 = 750,
                    Price100 = 2355,    
                    Description = "test1Updated"
                };

            // Act
            var result = await sut.PutProduct(1,product);
            var value = result.Value;
            
            
            
            // Assert

            Assert.IsType<ActionResult<Product>>(result);
            Assert.True(value.Name == "test1Updated");

        }
        [Fact]
        public async Task DeleteProduct_DeleteProductNotExists_ReturnsNotFound()
        {
            // Arrange

            var sut = new ProductsController(dbContext);
           
            // Act
            var result = await sut.DeleteProduct(10);
            
            // Assert


            Assert.IsType<NotFoundResult>(result);
        }

    }
}
