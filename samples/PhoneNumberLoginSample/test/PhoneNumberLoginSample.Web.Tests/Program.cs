using Microsoft.AspNetCore.Builder;
using PhoneNumberLoginSample;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<PhoneNumberLoginSampleWebTestModule>();

public partial class Program
{
}