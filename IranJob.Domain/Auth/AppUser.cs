using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using IranJob.Domain.Entities;

namespace IranJob.Domain.Auth
{
    public class AppUser : IdentityUser<string>
    {
        public string FullName { get; set; }
        public string ResumeFileName { get; set; }

        public List<JobRequest> JobRequests { get; set; }
    }
}
