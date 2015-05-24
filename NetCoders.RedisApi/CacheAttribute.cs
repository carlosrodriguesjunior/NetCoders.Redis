using Newtonsoft.Json;
using PostSharp.Aspects;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoders.RedisApi
{
    [Serializable]
    public class CacheAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379, abortConnect=false");

            if (redis.IsConnected)
            {
                IDatabase db = redis.GetDatabase();

                var result = db.StringGet(GetKeyCache(args));

                if (!result.IsNull)
                {
                    args.ReturnValue = JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
                    args.FlowBehavior = FlowBehavior.Return;
                }
            }

        }

        public override void OnExit(MethodExecutionArgs args)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379, abortConnect=false");

            if (redis.IsConnected)
            {
                IDatabase db = redis.GetDatabase();

                var key = GetKeyCache(args);

                db.StringSet(key, JsonConvert.SerializeObject(args.ReturnValue), TimeSpan.FromMinutes(10));
            }
        }

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
