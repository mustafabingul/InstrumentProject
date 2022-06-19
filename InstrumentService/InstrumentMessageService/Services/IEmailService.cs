using Instrument_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentMessageService
{
    public  interface IEmailService
    {
        void SendEmail(AlertRequest message);
    }
}
