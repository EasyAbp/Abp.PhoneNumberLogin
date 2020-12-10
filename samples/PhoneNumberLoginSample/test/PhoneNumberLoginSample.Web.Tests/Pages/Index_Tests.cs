using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace PhoneNumberLoginSample.Pages
{
    public class Index_Tests : PhoneNumberLoginSampleWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
