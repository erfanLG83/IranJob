using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IranJob.Domain.Entities;
using IranJob.Domain.Enums;
using IranJob.Services.Api;
using PrgHome.Web.Classes;

namespace IranJob.WebApi.Models
{
    public class JobRequestDetailsModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public bool IsSeened { get; set; }
        public string ApplicantMessage { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string Date { get; set; }
        public string JobStatus { get; set; }

        public JobRequestDetailsModel(JobRequest request)
        {
            ApplicantMessage = request.ApplicantMessage;
            PhoneNumber = request.PhoneNumber;
            Id = request.Id;
            JobId = request.JobId;
            JobTitle = $"{request.Job.Title} ({request.Job.Province.Name})";
            IsSeened = request.JobRequestStatus != JobRequestStatus.Unseen;
            CompanyName = request.Job.Company.Name;
            Date = ConvertDate.ConvertMiladiToShamsi(request.Date, "yyyy/MM/dd");
            JobStatus = request.JobRequestStatus == JobRequestStatus.Unseen
                ? null
                : request.JobRequestStatus.ToDisplay().First();
        }
    }
}
