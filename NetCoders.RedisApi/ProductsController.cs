using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NetCoders.RedisApi
{
    public class ProductsController : ApiController
    {

        public IEnumerable<Product> Get()
        {
             var watch = Stopwatch.StartNew();
            var lista =  new ProductRepository().GetAll();
            watch.Stop();
             System.Diagnostics.Debug.WriteLine("Tempo decimal Processamento :" + watch.ElapsedMilliseconds);
            return lista;
        }

        public string Get(int id)
        {
            return "value";
        }

        public void Post([FromBody]string value)
        {

            
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}