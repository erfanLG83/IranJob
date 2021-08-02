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
        public string PastTime { get; set; }
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
            int pastTimeDay = (DateTime.Now.Day * DateTime.Now.Month) - (job.PublishDate.Day * job.PublishDate.Month);
            if (pastTimeDay == 0)
                PastTime = "امروز";
            else if (pastTimeDay == 1)
                PastTime = "دیروز";
            else
                PastTime = $"{pastTimeDay} روز پیش";
            ImmediateEmployment = job.ImmediateEmployment;
            ContractType = job.ContractType;
            MinimumSalary = job.MinimumSalary;
            CompanyName = job.Company.Name;
            CompanyImage = ImagePaths.CompaniesPath + job.Company.ImageName;
            Province = job.Province.Name;
        }

    }
}
