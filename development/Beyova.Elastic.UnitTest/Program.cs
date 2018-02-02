using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.Api;

namespace Beyova.Elastic.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ElasticApiTracking(new ApiEndpoint
            {
                Host = "cn.e1elk-staging.ef.com",
                Port = 9200,
                Protocol = "http"
            }, "apitracking");

            var result = client.GetApiEventGroupStatistic(new ApiTracking.ApiEventGroupingCriteria { GroupByServerIdentifier = true, FromStamp = DateTime.UtcNow.AddDays(-7), FrameInterval = TimeScope.Hour });
        }
    }
}
