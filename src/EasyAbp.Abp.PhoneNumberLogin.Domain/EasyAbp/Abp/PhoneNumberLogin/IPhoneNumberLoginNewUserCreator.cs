using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public interface IPhoneNumberLoginNewUserCreator
    {
        Task<IdentityUser> CreateAsync(string phoneNumber, string userName = null, string password = null, string email = null);
    }
}