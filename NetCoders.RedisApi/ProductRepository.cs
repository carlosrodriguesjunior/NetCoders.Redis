using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoders.RedisApi
{
    public class ProductRepository
    {
        [Cache]
        public IEnumerable<Product> GetAll()
        {
            //System.Threading.Thread.Sleep(5000);
            using (var context = new Context())
            {
                return context.Products
                    .OrderBy(x => x.Description)
                    .ToList();
            }

        }
    }
}
