using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IranJob.Domain.Entities;

namespace IranJob.Services.Contract
{
    public interface IJobRepository
    {
        Task FinishTime(Job job);
    }
}
