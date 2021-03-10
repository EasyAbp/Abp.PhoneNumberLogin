using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    public enum TryRegisterAndRequestTokenResultType : byte
    {
        Login = 1,

        Register = 2
    }
}
