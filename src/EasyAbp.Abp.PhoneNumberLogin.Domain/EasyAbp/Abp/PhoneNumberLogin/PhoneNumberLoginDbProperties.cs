namespace EasyAbp.Abp.PhoneNumberLogin
{
    public static class PhoneNumberLoginDbProperties
    {
        public static string DbTablePrefix { get; set; } = "EasyAbpAbpPhoneNumberLogin";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "EasyAbpAbpPhoneNumberLogin";
    }
}
