namespace EasyAbp.Abp.PhoneNumberLogin
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum VerificationCodeType : byte
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login = 1,

        /// <summary>
        /// 注册
        /// </summary>
        Register = 2,

        /// <summary>
        /// 重置密码
        /// </summary>
        ResetPassword = 3,

        /// <summary>
        /// 确认手机
        /// </summary>
        Confirm = 4
    }
}
