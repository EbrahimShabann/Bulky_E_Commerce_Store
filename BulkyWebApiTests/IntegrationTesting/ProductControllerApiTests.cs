using Bulky.Models.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BulkyWebApi.Test.IntegrationTesting
{
    public class ProductControllerApiTests : IClassFixture<ProductsMockServer>
    {

        private readonly HttpClient client;
       private readonly ProductsMockServer _mockServer;
        
        public ProductControllerApiTests(ProductsMockServer mockServer)
        {

            _mockServer = mockServer;
            _mockServer.Start();
            
            client = new HttpClient { BaseAddress = new Uri(_mockServer.Url) };
        }


        [Fact]
        public async Task GetAllEndpoint_TakeRequestToGetProducts_ReturnsStatusCodeOkAndListOfProducts()
        {
            //Arrange

            _mockServer.CreateGetAllStup("/api/Products/GetProducts");

            //Act

            var response = await client.GetAsync("/api/Products/GetProducts");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(content);


            //Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<List<Product>>(products);
            Assert.Equal(3, products.Count);
        } 
        [Fact]
        public async Task GetByIdEndpoint_TakeRequestToGetProductByIdNotExists_ReturnsNotFound()
        {

            //Arrange
            _mockServer.CreateGetByIdStup(5);

            //Act

            var response = await client.GetAsync("/api/Products/GetProduct/5");
           
            


            //Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
           
        }
        [Fact]
        public async Task GetByIdEndpoint_TakeRequestToGetProductById_ReturnsProduct()
        {

            //Arrange
            _mockServer.CreateGetByIdStup(1);

            //Act

            var response = await client.GetAsync("/api/Products/GetProduct/1");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);
            

            //Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<Product>(product);
            Assert.Equal("PantsTest", product.Name);

        }
        [Fact]
        public async Task PostEndpoint_TakeRequestToAddProduct_ReturnsProduct()
        {

            //Arrange
            Product NewProduct = new() { Id = 4, Name = "Test4", Brand = "Test4", ListPrice = 50, Price1 = 85, Price50 = 750, Price100 = 2355, CategoryId = 1, Description = "Test4" };
            _mockServer.CreatePostStup(NewProduct);
            string JsonProduct = JsonConvert.SerializeObject(NewProduct);
            var HttpContent = new StringContent(JsonProduct, Encoding.UTF8, "application/json");


            //Act

            var response = await client.PostAsync ("/api/Products/PostProduct" , HttpContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(content);
            

            //Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(4, products.Count);
            Assert.Equal("Test4", products[3].Name);

        }
        [Fact]
        public async Task DeleteEndpoint_TakeRequestToRemoveProduct_ReturnsNotFound()
        {

            //Arrange
            _mockServer.CreateDeleteStup(5);
           


            //Act

            var response = await client.DeleteAsync("/api/Products/DeleteProduct/5");
            
            

            //Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            

        }
        [Fact]
        public async Task DeleteEndpoint_TakeRequestToRemoveProduct_ReturnsProductsAfterRemovingTheRemovedProduct()
        {

            //Arrange
            _mockServer.CreateDeleteStup(3);
            


            //Act

            var response = await client.DeleteAsync("/api/Products/DeleteProduct/3");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(content);


            //Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, products.Count);
            Assert.DoesNotContain("T-ShirtsTest", content);
        }
        [Fact]
        public async Task PutEndpoint_TakeRequestToEditProductWithIdIsNotProductId_ReturnsBadRequest()
        {

            //Arrange
            Product UpdatedProduct = new() 
            { Id = 4, Name = "Updated Product", Brand = "Updated Product", ListPrice = 50, Price1 = 85, Price50 = 750, Price100 = 2355, CategoryId = 1, Description = "Updated Product" };
            _mockServer.CreatePutStup(5,UpdatedProduct);



            //Act
            var JsonProduct = JsonConvert.SerializeObject(UpdatedProduct);
            var ContentProduct= new StringContent(JsonProduct, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/Products/PutProduct/5",ContentProduct);



            //Assert

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


        }
        [Fact]
        public async Task PutEndpoint_TakeRequestToEditProductNotExists_ReturnsNotFound()
        {

            //Arrange
            Product UpdatedProduct = new() 
            { Id = 4, Name = "Updated Product", Brand = "Updated Product", ListPrice = 50, Price1 = 85, Price50 = 750, Price100 = 2355, CategoryId = 1, Description = "Updated Product" };
            _mockServer.CreatePutStup(4,UpdatedProduct);



            //Act
            var JsonProduct = JsonConvert.SerializeObject(UpdatedProduct);
            var ContentProduct= new StringContent(JsonProduct, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/Products/PutProduct/4",ContentProduct);



            //Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);


        }
        [Fact]
        public async Task PutEndpoint_TakeRequestToEditProduct_ReturnsUpdatedProduct()
        {

            //Arrange
            Product UpdatedProduct = new() 
            { Id = 3, Name = "Updated Product", Brand = "Updated Product", ListPrice = 50, Price1 = 85, Price50 = 750, Price100 = 2355, CategoryId = 1, Description = "Updated Product" };
            _mockServer.CreatePutStup(3,UpdatedProduct);



            //Act
            var JsonProduct = JsonConvert.SerializeObject(UpdatedProduct);
            var ContentProduct= new StringContent(JsonProduct, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/Products/PutProduct/3",ContentProduct);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(content);



            //Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Updated Product", products[2].Name);


        }
    }
}
