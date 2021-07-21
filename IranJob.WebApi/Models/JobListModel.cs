using System;
using IranJob.Domain.Entities;
using IranJob.Domain.Enums;
using IranJob.Services;
using IranJob.Services.Api;

namespace IranJob.WebApi.Models
{
    public class JobListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PastTime { get; set; }
        public bool ImmediateEmployment { get; set; }
        public EnumObject ContractType { get; set; }
        public EnumObject MinimumSalary { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImage { get; set; }
        public string Province { get; set; }


        public JobListModel(Job job)
        {
            Id = job.Id;
            Title = job.Title;
            PastTime = (DateTime.Now.Day * DateTime.Now.Month) - (job.PublishDate.Day * job.PublishDate.Month);
            ImmediateEmployment = job.ImmediateEmployment;
            ContractType = job.ContractType;
            MinimumSalary = job.MinimumSalary;
            CompanyName = job.Company.Name;
            CompanyImage = job.Company.ImageName;
            Province = job.Province.Name;
        }

    }
}
