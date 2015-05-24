using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

namespace NetCoders.RedisApi
{
    public class ProductsController : ApiController
    {

        //Nossa api exposta para o retorno de dados
        public IEnumerable<Product> Get()
        {
            //StopWatch é uma classe para medição de tempo
            //Iniciamos a medição de tempo
            var watch = Stopwatch.StartNew();

            //Instanciamos o nosso repositorio de dados de Produtos e obtemos os dados
            var list =  new ProductRepository().GetAll();

            //Paramos a medição de tempo
            watch.Stop();

            //Exibimos no output do servidor o tempo de processamento do método de obtenção de dados
            Debug.WriteLine("Tempo Processamento :" + watch.ElapsedMilliseconds);

            //Retorna os dados para a nossa api
            return list;
        }
    }
}