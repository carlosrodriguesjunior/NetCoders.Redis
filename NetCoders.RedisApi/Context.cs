using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace NetCoders.RedisApi
{
    //Contexto de Dados do EntityFramework
    public class Context : DbContext
    {
        public Context()
            : base("ContextDb")
        {
        }
        public DbSet<Product> Products { get; set; }

    }

    //running Update-Database
    //Seed de nossa base de dados
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
                    Value = 100.0F
                });

                context.SaveChanges();

            }
            

        }
    }

}
