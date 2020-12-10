# Abp.PhoneNumberLogin

[![NuGet](https://img.shields.io/nuget/v/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared)
[![NuGet Download](https://img.shields.io/nuget/dt/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared.svg?style=flat-square)](https://www.nuget.org/packages/EasyAbp.Abp.PhoneNumberLogin.Domain.Shared)
[![GitHub stars](https://img.shields.io/github/stars/EasyAbp/Abp.PhoneNumberLogin?style=social)](https://www.github.com/EasyAbp/Abp.PhoneNumberLogin)

An abp module to avoid duplicate user phone numbers being confirmed and providing phone number confirmation and phone number login features and more.

## Online Demo

We have launched an online demo for this module: [https://phonelogin.samples.easyabp.io](https://phonelogin.samples.easyabp.io)

## Installation

1. Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/How-To.md#add-nuget-packages))

    * EasyAbp.Abp.PhoneNumberLogin.Application
    * EasyAbp.Abp.PhoneNumberLogin.Application.Contracts
    * EasyAbp.Abp.PhoneNumberLogin.Domain
    * EasyAbp.Abp.PhoneNumberLogin.Domain.Shared
    * EasyAbp.Abp.PhoneNumberLogin.EntityFrameworkCore
    * EasyAbp.Abp.PhoneNumberLogin.HttpApi
    * EasyAbp.Abp.PhoneNumberLogin.HttpApi.Client
    * (Optional) EasyAbp.Abp.PhoneNumberLogin.MongoDB
    * (Optional) EasyAbp.Abp.PhoneNumberLogin.Web

1. Add `DependsOn(typeof(Abp.PhoneNumberLoginXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/main/How-To.md#add-module-dependencies))

1. Add `builder.ConfigureAbpPhoneNumberLogin();` to the `OnModelCreating()` method in **MyProjectMigrationsDbContext.cs**.

1. Add EF Core migrations and update your database. See: [ABP document](https://docs.abp.io/en/abp/latest/Tutorials/Part-1?UI=MVC#add-new-migration-update-the-database).

## Usage

### Razor Pages Phone Number Login

Ensure the `EasyAbp.Abp.PhoneNumberLogin.Web` module was installed.

1. Customize the default login page ([see demo](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/samples/PhoneNumberLoginSample/src/PhoneNumberLoginSample.Web/Pages/Account)).

2. Register an account with a confirmed phone number or confirm your phone number of your existing account.

4. Log out and try to log in by phone number and password (or verification code).

### Identity Server Token Endpoint

* By Password

    1. Request `/api/phone-number-login/account/request-token/by-password` in POST method. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/RequestTokenByPasswordInput.cs))

* By Verification Code

    1. Request `/api/phone-number-login/account/send-verification-code` in POST method to send and receive a verification code for confirming a phone number. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/SendVerificationCodeInput.cs))
    2. Request `/api/phone-number-login/account/request-token/by-verification-code` in POST method. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/RequestTokenByVerificationCodeInput.cs))

* Refresh a Token

    1. Request `/api/phone-number-login/account/refresh-token` in POST method. ([see input model](https://github.com/EasyAbp/Abp.PhoneNumberLogin/blob/main/src/EasyAbp.Abp.PhoneNumberLogin.Application.Contracts/EasyAbp/Abp/PhoneNumberLogin/Account/Dtos/RefreshTokenInput.cs))

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
