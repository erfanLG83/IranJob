using IranJob.Persistence;
using IranJob.Services.Api;
using IranJob.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IranJob.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class JobRequestsController : ControllerBase
    {
        private readonly IranJobDbContext _context;

        public JobRequestsController(IranJobDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ApiResult<IEnumerable<JobRequestModel>>> GetAllRequests()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return BadRequest();
            var requests = await _context.JobRequests
                .Where(x => x.UserId == userId)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Company)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Province)
                .ToListAsync();
            return requests.Select(x => new JobRequestModel(x)).ToList();
        }
    }
}