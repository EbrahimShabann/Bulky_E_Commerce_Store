using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWebApi.Test.IntegrationTesting
{
    public interface IMockServer
    {
        void Start();
        void Stop();
        void CreateGetAllStup(string path);
        void CreateGetByIdStup(int id);
        void CreatePostStup(Product product );
        void CreateDeleteStup(int id);
        void CreatePutStup(int id, Product product);

    }
}
