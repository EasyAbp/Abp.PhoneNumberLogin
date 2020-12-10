using Volo.Abp.Reflection;

namespace EasyAbp.Abp.PhoneNumberLogin.Permissions
{
    public class PhoneNumberLoginPermissions
    {
        public const string GroupName = "EasyAbp.Abp.PhoneNumberLogin";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(PhoneNumberLoginPermissions));
        }
        
        public static class UserLookup
        {
            public const string Default = GroupName + ".UserLookup";
        }
    }
}