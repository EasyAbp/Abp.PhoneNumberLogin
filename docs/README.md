# Abp.PhoneNumberLogin

[![ABP version](https://img.shields.io/badge/dynamic/xml?style=flat-square&color=yellow&label=abp&query=%2F%2FProject%2FPropertyGroup%2FAbpVersion&url=https%3A%2F%2Fraw.githubusercontent.com%2FEasyAbp%2FAbp.PhoneNumberLogin%2Fmain%2FDirectory.Build.props)](https://abp.io)
[![NuGet](https://img.shields.io/nuget/v/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared)
[![Discord online](https://badgen.net/discord/online-members/xyg8TrRa27?label=Discord)](https://discord.gg/xyg8TrRa27)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/Abp.PhoneNumberLogin?style=social)](https://www.github.com/EasyAbp/Abp.PhoneNumberLogin)

An abp module to avoid duplicate user phone numbers being confirmed and providing phone number confirmation and phone number login features and more.

## Installation

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-nuget-packages))

    * EasyAbp.Abp.PhoneNumberLogin.Application
    * EasyAbp.Abp.PhoneNumberLogin.Application.Contracts
    * (Exclusive) EasyAbp.Abp.PhoneNumberLogin.Domain.OpenIddict
    * (Exclusive) EasyAbp.Abp.PhoneNumberLogin.Domain.Ids4
    * EasyAbp.Abp.PhoneNumberLogin.Domain.Shared
    * EasyAbp.Abp.PhoneNumberLogin.EntityFrameworkCore
    * EasyAbp.Abp.PhoneNumberLogin.HttpApi
    * EasyAbp.Abp.PhoneNumberLogin.HttpApi.Client
    * (Optional) EasyAbp.Abp.PhoneNumberLogin.MongoDB
    * (Optional) EasyAbp.Abp.PhoneNumberLogin.Web

1. Add `DependsOn(typeof(Abp.PhoneNumberLoginXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-module-dependencies))

1. Add `builder.ConfigurePhoneNumberLogin();` to the `OnModelCreating()` method in **MyProjectMigrationsDbContext.cs**.

1. Add EF Core migrations and update your database. See: [ABP document](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC&DB=EF#add-database-migration).

1. Specify a auth server in the appsettings.json file of the Web/Host project.
   ```json
   {
     "AbpPhoneNumberLogin": {
       "AuthServer": {
         "Authority": "https://localhost:44395",
         "ClientId": "MyProjectName_App",
         "ClientSecret": "1q2w3e*"
       }
     }
   }
   ```

1. Add the `PhoneNumberLogin_credentials` grant type in OpenIddictDataSeedContributor.
    ```CSharp
    grantTypes: new List<string>
    {
        OpenIddictConstants.GrantTypes.AuthorizationCode,
        OpenIddictConstants.GrantTypes.Password,
        OpenIddictConstants.GrantTypes.ClientCredentials,
        OpenIddictConstants.GrantTypes.RefreshToken,
        PhoneNumberLoginConsts.GrantType // add this grant type
    }
    ```

1. Find these cocdes in OpenIddictDataSeedContributor
    ```CSharp
    if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
    {
        application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
    }
    ```
    and add these codes on the following line
    ```CSharp
    if (grantType == WeChatMiniProgramConsts.GrantType)
    {
        application.Permissions.Add($"gt:{WeChatMiniProgramConsts.GrantType}");
    }
    ```

1. Run the DbMigrator project to create the client. Notice that you must manually add the grant type if your client already exists.

## Usage

### Razor Pages Phone Number Login

Ensure the `EasyAbp.Abp.PhoneNumberLogin.Web` module was installed.

1. Customize the default login page ([see demo](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/samples/PhoneNumberLoginSample/src/PhoneNumberLoginSample.Web/Pages/Account)).

2. Register an account with a confirmed phone number or confirm your phone number of your existing account.

3. Log out and try to log in by phone number and password (or verification code).

### Identity Server Token Endpoint

* By Password

    1. Request `/api/phone-number-login/account/request-token/by-password` in POST method. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/RequestTokenByPasswordInput.cs))

* By Verification Code

    1. Request `/api/phone-number-login/account/send-verification-code` in POST method to send and receive a verification code for confirming a phone number. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/SendVerificationCodeInput.cs))
    2. Request `/api/phone-number-login/account/request-token/by-verification-code` in POST method. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/RequestTokenByVerificationCodeInput.cs))

* Refresh a Token

    * Request `/api/phone-number-login/account/refresh-token` in POST method. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/RefreshTokenInput.cs))
    * You can also use `/connect/token` to refresh a token.

![LoginByPhoneNumberAndPassword](/docs/images/LoginByPhoneNumberAndPassword.png)

## Road map

- [x] Keep allowing users to set a phone number that has been used by others.
- [x] Only confirmed phone numbers are allowed to be used for login.
- [x] Avoid setting a non-numeric phone number.
- [x] Avoid setting a phone number starting with 0.
- [x] Avoid duplicate user phone numbers being confirmed.
- [x] Razor pages log in by phone number and password widget.
- [ ] Razor pages log in by phone number and verification code widget.
- [ ] Request token by phone number and password.
- [ ] Request token by phone number and verification code.
- [ ] Simply generate and send verification codes.
- [ ] User confirm Phone number with verification code.
- [ ] Register an account with phone number and verification code.
- [ ] Reset password with phone number and verification code.
- [ ] Using EasyAbp.Abp.VerificationCode module to generate verification codes.
- [ ] Support EasyAbp integrated login module.
- [ ] Unit tests.
