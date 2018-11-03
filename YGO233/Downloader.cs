using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Cache;

namespace YGO233
{
    public class Downloader
    {
        public static void GetStringAsync(string url, Func<string, int> callback, Func<Exception, int> fail)
        {
            YGO233WebClient client = new YGO233WebClient();
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    fail(e.Error);
                else
                    callback(e.Result);
            };
            client.DownloadStringAsync(new Uri(url));
        }
    }

    public class YGO233WebClient: WebClient
    {
        public YGO233WebClient()
        {
            Proxy = null;
            Credentials = CredentialCache.DefaultCredentials;
            CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            Headers.Add(HttpRequestHeader.UserAgent, "YGO233");
            Encoding = Encoding.UTF8;
        }
    }
}
