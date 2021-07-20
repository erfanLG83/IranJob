using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IranJob.Services.Contract
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
}
