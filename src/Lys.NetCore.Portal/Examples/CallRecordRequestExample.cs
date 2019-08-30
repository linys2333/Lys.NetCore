using Lys.NetCore.Portal.DTOs;
using Swashbuckle.AspNetCore.Examples;

namespace Lys.NetCore.Portal.Examples
{
    public class CallRecordRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CallRecordRequest
            {
                Mobile = "13900139000",
                Duration = 60
            };
        }
    }
}
