using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoders.RedisApi
{
    public class Context : DbContext
    {
        public Context()
            : base("ContextDb")
        {
        }
        public DbSet<Product> Products { get; set; }

    }

    //running Update-Database
    public class ProductConfiguration : DbMigrationsConfiguration<Context>
    {
        public ProductConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(Context context)
        {
            for (int i = 0; i < 5000; i++)
            {
                context.Products.Add(new Product
                {
                    Description = "Produto " + i,
                    Value = 100.0F,
                    Observation = Guid.NewGuid().ToString()
                });

                context.SaveChanges();

            }
            

        }
    }

}
