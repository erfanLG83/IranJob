using System;
using IranJob.Domain.Entities;
using IranJob.Domain.Enums;

namespace IranJob.WebApi.Models
{
    public class JobListModel
    {
        public string Title { get; set; }
        public int PastTime { get; set; }
        public bool ImmediateEmployment { get; set; }
        public ContractType ContractType { get; set; }
        public MinimumSalary MinimumSalary { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImage { get; set; }
        public string Province { get; set; }


        public JobListModel(Job job)
        {
            Title = job.Title;
            PastTime = (DateTime.Now.Day * DateTime.Now.Month) - (job.PublishDate.Month * job.PublishDate.Month);
            ImmediateEmployment = job.ImmediateEmployment;
            ContractType = job.ContractType;
            MinimumSalary = job.MinimumSalary;
            CompanyName = job.Company.Name;
            CompanyImage = job.Company.ImageName;
            Province = job.Province.Name;
        }

    }
}
