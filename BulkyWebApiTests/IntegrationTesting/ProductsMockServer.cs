using Bulky.Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace BulkyWebApi.Test.IntegrationTesting
{
    public class ProductsMockServer : IMockServer

    {
       
        private WireMockServer? _server;
        public string Url => _server.Url;
        private readonly InMemoryDbContext? _context=new();

        public ProductsMockServer()
        {
            
            List<Product> TestProducts = new()
            {
                    new() { Id = 1, Name = "PantsTest", Brand = "GucciTest", ListPrice = 50, Price1 = 85, Price50 = 750, Price100 = 2355, CategoryId = 1, Description = "men ClothesTest" },
                new() { Id = 2, Name = "ShirtsTest", Brand = "GucciTest", ListPrice = 55, Price1 = 90, Price50 = 800, Price100 = 2500, CategoryId = 2, Description = "Women ClothesTest" },
                new() { Id = 3, Name = "T-ShirtsTest", Brand = "GucciTest", ListPrice = 45, Price1 = 75, Price50 = 750, Price100 = 2300, CategoryId = 1, Description = "men ClothesTest" }
            };
            _context.Products.AddRange(TestProducts);
            _context.SaveChanges();
        }

        public void Start()
        {
            _server = WireMockServer.Start();
        }

        public  void CreateGetAllStup(string path)
        {
            string JsonProducts = JsonConvert.SerializeObject(_context.Products);
           
            _server.Given(
                Request.Create().WithPath(path).UsingGet()
            )
            .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBody(JsonProducts)
            );
        }
        
        public void Stop()
        {
            _server.Stop();
        }

        public void CreateGetByIdStup(int id)
        {
            var product = _context.Products.Find(id);
            var JsonProduct= JsonConvert.SerializeObject(product);
            if (product == null)
            {
                _server.Given(
                          Request.Create().WithPath($"/api/Products/GetProduct/{id}").UsingGet()
                        )
                        .RespondWith(
                            Response.Create()
                            .WithStatusCode(404)
                        );
            }
            else
            { 

            _server.Given(
                          Request.Create().WithPath($"/api/Products/GetProduct/{id}").UsingGet()
                        )
                        .RespondWith(
                            Response.Create()
                            .WithStatusCode(200)
                            .WithBody(JsonProduct)
                        );
            }
        }

        public void CreatePostStup(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
            var JsonProducts = JsonConvert.SerializeObject(_context.Products);
            _server.Given(
                Request.Create().WithPath("/api/Products/PostProduct").UsingPost()
            )
            .RespondWith(
                Response.Create()
                .WithStatusCode(200)
                .WithBody(JsonProducts)
            );

        }
        public void CreateDeleteStup(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                _server.Given(
                          Request.Create().WithPath($"/api/Products/DeleteProduct/{id}").UsingDelete()
                        )
                        .RespondWith(
                            Response.Create()
                            .WithStatusCode(404)
                        );
            }
            else
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                var JsonProducts = JsonConvert.SerializeObject(_context.Products);
                _server.Given(
                    Request.Create().WithPath($"/api/Products/DeleteProduct/{id}").UsingDelete()
                )
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(200)
                    .WithBody(JsonProducts)
                );
            }
        }
        public void CreatePutStup(int id, Product product)
        {
            if (id != product.Id)
            {
                _server.Given(
                          Request.Create().WithPath($"/api/Products/PutProduct/{id}").UsingPut()
                        )
                        .RespondWith(
                            Response.Create()
                            .WithStatusCode(400)
                        );
            }
            else 
            { 
                    var oldProduct = _context.Products.Find(id);
                
                    if (oldProduct == null)
                    {
                        _server.Given(
                                  Request.Create().WithPath($"/api/Products/PutProduct/{id}").UsingPut()
                                )
                                .RespondWith(
                                    Response.Create()
                                    .WithStatusCode(404)
                                );
                    }
                    else
                    {

                    oldProduct.Name=product.Name;
                    oldProduct.Brand = product.Brand;
                    oldProduct.ListPrice = product.ListPrice;
                    oldProduct.Price1 = product.Price1;
                    oldProduct.Price50 = product.Price50;
                    oldProduct.Price100 = product.Price100;
                    oldProduct.CategoryId = product.CategoryId;
                    oldProduct.Description = product.Description;

                    _context.Products.Update(oldProduct);
                    _context.SaveChanges();
                        var JsonProducts = JsonConvert.SerializeObject(_context.Products);
                        _server.Given(
                            Request.Create().WithPath($"/api/Products/PutProduct/{id}").UsingPut()
                        )
                        .RespondWith(
                            Response.Create()
                            .WithStatusCode(200)
                            .WithBody(JsonProducts)
                        );
                    }
            }
        }
    }                  
}
