﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Domain.Models
{
    public class SuccessResponse
    {
        public bool Success { get; set; } 
        public string ResponseCode{ get; set; }   

        public string Message { get; set; }
    }
}
