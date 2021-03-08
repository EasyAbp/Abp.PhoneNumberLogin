using AutoMapper;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using Volo.Abp.Identity;

namespace EasyAbp.Abp.PhoneNumberLogin
{
    public class PhoneNumberLoginApplicationAutoMapperProfile : Profile
    {
        public PhoneNumberLoginApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<IdentityUser, IdentityUserDto>()
                .MapExtraProperties();
        }
    }
}