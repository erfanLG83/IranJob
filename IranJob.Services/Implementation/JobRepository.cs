using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IranJob.Domain.Entities;
using IranJob.Persistence;
using IranJob.Services.Contract;

namespace IranJob.Services.Implementation
{
    public class JobRepository:IJobRepository
    {
        private readonly IranJobDbContext _context;

        public JobRepository(IranJobDbContext context)
        {
            _context = context;
        }
        public async Task FinishTime(Job job)
        {
            if (job.Finished)
                await Task.CompletedTask;
            job.Finished = true;
            job.EndTimeJobId = null;
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }
    }
}
