using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Domain.Models
{
    public class AlertRequest
    {
        public long Id { get; set; }    
        public string Email { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public int StockId { get; set; }
    }
}
