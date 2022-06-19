using Instrument_Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentMessageService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;  
        }
        public void SendEmail(AlertRequest message)
        {
            Send(message);
        }

        private void Send(AlertRequest mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.UseDefaultCredentials = true;
                    client.Send(_config.GetValue<string>("EmailConfiguration:From"), mailMessage.Email, "Stock price Alert", mailMessage.Price.ToString());
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }
    }
}
