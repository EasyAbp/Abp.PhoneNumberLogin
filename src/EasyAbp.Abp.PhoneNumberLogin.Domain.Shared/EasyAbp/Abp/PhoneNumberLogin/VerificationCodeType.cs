namespace EasyAbp.Abp.PhoneNumberLogin
{
    public enum VerificationCodeType : byte
    {
        Login = 1,
        
        Register = 2,
        
        ResetPassword = 3,
        
        Confirm = 4
    }
}