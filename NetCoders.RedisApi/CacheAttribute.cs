using NServiceKit.Redis;
using PostSharp.Aspects;
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
            //Conectando no Redis utilizando o NserviceKit adicionar a Using NServiceKit.Redis;
            using (var redis = new RedisClient("127.0.0.1", 6379))
            {

                    //Obtemos os dados da nossa Key-chave no redis
                    var result = redis.Get<IEnumerable<Product>>(GetKeyCache(args));

                    //Se o resultado não estiver nulo, entao temos dados em e vamos retorna-lo sem que o Metodo em questão seja executado
                    if (result !=null)
                    {
                        //Pegamos o valor obtido no cache e informamos para nossa AOP que vamos retornar esses valores
                        args.ReturnValue = result;
                        //Informamos para a nossa AOP que não precisa executar o método em questão e retornar os valores obtido no cache
                        args.FlowBehavior = FlowBehavior.Return;
                    }
            }

        }

        //Sobreescrevemos o OnExit para interceptarmos os dados que vão ser retornados no metodo cacheado e guardamos no Redis
        public override void OnExit(MethodExecutionArgs args)
        {
            //Conectamos no Redis  com o NserviceKit
            using (var redis = new RedisClient("127.0.0.1", 6379))
            {
                //Obtemos nossa chave
                var key = GetKeyCache(args);
                
                //Guardamos o retorno do método no Cache
                redis.Set<IEnumerable<Product>>(GetKeyCache(args), (IEnumerable<Product>)args.ReturnValue, TimeSpan.FromMinutes(10));
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
