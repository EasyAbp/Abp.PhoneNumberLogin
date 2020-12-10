using System;
using System.Collections.Generic;
using System.Text;
using PhoneNumberLoginSample.Localization;
using Volo.Abp.Application.Services;

namespace PhoneNumberLoginSample
{
    /* Inherit your application services from this class.
     */
    public abstract class PhoneNumberLoginSampleAppService : ApplicationService
    {
        protected PhoneNumberLoginSampleAppService()
        {
            LocalizationResource = typeof(PhoneNumberLoginSampleResource);
        }
    }
}
