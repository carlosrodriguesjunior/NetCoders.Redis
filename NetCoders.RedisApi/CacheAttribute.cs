using Newtonsoft.Json;
using PostSharp.Aspects;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoders.RedisApi
{
    //Classe que irá gerar um atributo a ser colocado nos metodos que farão cache
    //Para alterarmos os comportamentos dos métodos que através de Orientação a Aspecto,
    //nossas precisam herdar OnMethodBoundaryAspect que está no namespace PostSharp.Aspects e temos que adiciona-lo na Using.
    //Vamos precisar Serializar os dados para poder guardar no Redis então devemos adicionar o Atributo [Serializable] em nossa classe
    [Serializable]
    public class CacheAttribute : OnMethodBoundaryAspect
    {

        //Sobrescrevemos o OnEntry para interceptarmos  a entrada da execução do método a ser cacheado
        public override void OnEntry(MethodExecutionArgs args)
        {
            //Estabelecendo conexão com o redis através do StackExchange.Redis e adcionamos sua using
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379, abortConnect=false");

            //Verificamos se a conexão foi estabelecida com sucesso
            if (redis.IsConnected)
            {
                //Criamos o objeto de dados do Redis
                IDatabase db = redis.GetDatabase();

                //Obtemos os dados da nossa Key-chave
                var result = db.StringGet(GetKeyCache(args));

                //Se o resultado não estiver nulo, entao temos dados em e vamos retorna-lo sem que o Metodo em questão seja executado
                if (!result.IsNull)
                {
                    //Pegamos o valor obtido no cache e informamos para nossa AOP que vamos retornar esses valores
                    args.ReturnValue = JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
                    //Informamos para a nossa AOP que não precisa executar o método em questão e retornar os valores obtido no cache
                    args.FlowBehavior = FlowBehavior.Return;
                }
            }

        }

        //Sobreescrevemos o OnExit para interceptarmos os dados que vão ser retornados no metodo cacheado e guardamos no Redis
        public override void OnExit(MethodExecutionArgs args)
        {
            //Estabelece Conexão com o Redis
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379, abortConnect=false");

            if (redis.IsConnected)
            {
                IDatabase db = redis.GetDatabase();

                var key = GetKeyCache(args);

                //Guarda o valor retornado no Redis para que o mesmo seja cacheado
                db.StringSet(key, JsonConvert.SerializeObject(args.ReturnValue), TimeSpan.FromMinutes(10));
            }
        }

        //Metodo que uma Key para utilizarmos no cache
        public string GetKeyCache(MethodExecutionArgs args)
        {
            var key = new StringBuilder(args.Method.Name);
            var parameters = args.Method.GetParameters();
            key.Append(":");
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                    key.Append(":");

                key.Append(parameters[i].Name);
                key.Append(":");
                key.Append(args.Arguments[i]);
            }

            return key.ToString();
        }
    }
}
