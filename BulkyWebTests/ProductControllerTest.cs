using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using BulkyWeb.Areas.Admin.Controllers;
using BulkyWebApi.Test.IntegrationTesting;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Newtonsoft.Json;
using System.Net;


namespace BulkyWeb.Test
{
    public class ProductControllerTest : IClassFixture<ProductsMockServer>
    {

        private readonly ProductsMockServer? _mockServer;
        private readonly HttpClient? client;
        public ProductControllerTest(ProductsMockServer mockServer)
        {
            _mockServer = mockServer;
            _mockServer.Start();
            client= new HttpClient { BaseAddress = new Uri(_mockServer.Url) };

        }

        [Fact]
        public async Task IndexAction_TakeRequest_ReturnsProducts()
        {
            //Arrange
            
            _mockServer.CreateGetAllStup("/Admin/ProductController/Index");


            //Act

            var respponse = await client.GetAsync("/Admin/ProductController/Index");
            respponse.EnsureSuccessStatusCode();
            var JsonProducts= await respponse.Content.ReadAsStringAsync();
            var Products= JsonConvert.DeserializeObject<List<Product>>(JsonProducts);

            //Assert
            Assert.Equal(HttpStatusCode.OK, respponse.StatusCode);
            Assert.IsType<List<Product>>(Products);
            Assert.Equal(3, Products.Count);
        }

    }
}
