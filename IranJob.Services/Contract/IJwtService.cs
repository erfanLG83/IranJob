using System.Threading.Tasks;
using IranJob.Domain.Auth;

namespace IranJob.Services.Contract
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(AppUser user);
    }
}
