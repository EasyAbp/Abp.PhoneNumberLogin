using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAbp.Abp.PhoneNumberLogin.Account.Dtos
{
    public enum RegisterResult : byte
    {
        UserAlreadyExists = 1,

        RegistrationSuccess = 2
    }
}
