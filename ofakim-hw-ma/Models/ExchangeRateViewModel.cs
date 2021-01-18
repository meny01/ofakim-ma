using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ofakim_hw_ma.Models
{
    public class ExchangeRateViewModel
    {
        public string ExName { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
