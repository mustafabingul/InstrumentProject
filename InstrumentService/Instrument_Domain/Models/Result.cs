using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Domain.Models
{
    public class Result<T>
    {
        public T Data { get; set; }
        public List<T> List { get; set; }
        public long IdentityValue { get; set; }
        public int ReturnValue { get; set; }
        public bool Error { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long NoOfRecords { get; set; }
    }
}