using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoders.RedisApi
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public float Value { get; set; }

        public string Observation { get; set; }
    }
}
