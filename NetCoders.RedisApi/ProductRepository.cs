using System.Collections.Generic;
using System.Linq;

namespace NetCoders.RedisApi
{
    //Repositorio de Dados a ser cacheado
    public class ProductRepository
    {
        //Colocamos nosso atributo de [Cache] para que o mesmo através de AOP possa
        //fazer cache no Redis
        [Cache]
        public IEnumerable<Product> GetAll()
        {
            //System.Threading.Thread.Sleep(1000);

            //Instancia o nosso objeto de Contexto do EntityFramework
            using (var context = new Context())
            {
                //Executamos a query de obtenção de dados de Produto atravé do EntityFrameWork
                return context.Products
                    .OrderBy(x => x.Description)
                    .ToList();
            }

        }
    }
}
