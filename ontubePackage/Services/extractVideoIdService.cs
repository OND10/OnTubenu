using ontubePackage.Common.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ontubePackage.Services
{
    public class extractVideoIdService
    {
        public async Task<Result<string>> ExtractYTubeId(string youtubeUrl)
        {
            var uri = new Uri(youtubeUrl);
            var query = uri.Query.TrimStart('?');
            var queryParams = query.Split('&');
            foreach (var param in queryParams)
            {
                var parts = param.Split('=');
                if (parts.Length == 2 && parts[0] == "v")
                {
                    return await Result<string>.SuccessAsync(parts[1], "Extracted Successfully", true);
                }
            }
            return await Result<string>.FaildAsync(false, "Invalid YouTube URL: no video ID found.");
        }
    }
}
