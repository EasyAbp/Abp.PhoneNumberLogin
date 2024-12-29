using Microsoft.AspNetCore.Builder;
using PhoneNumberLoginSample;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();

builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("PhoneNumberLoginSample.Web.csproj");
await builder.RunAbpModuleAsync<PhoneNumberLoginSampleWebTestModule>(applicationName: "PhoneNumberLoginSample.Web" );

public partial class Program
{
}