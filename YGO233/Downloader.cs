using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Cache;

namespace YGO233
{
    public static class Downloader
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

        public static void DownloadFileAsync(string url, string name, string path, Func<string, int> callback, Func<Exception, int> fail)
        {
            YGO233WebClient client = new YGO233WebClient();
            client.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    fail(e.Error);
                else
                    callback(name);
            };
            client.DownloadFileAsync(new Uri(url), path);
        }

        public static void DownloadFileAsync(DownloadTask task, Func<string, int> callback, Func<Exception, int> fail)
        {
            DownloadFileAsync(task.Url, task.Name, task.Path, callback, fail);
        }
    }

    public class DownloadTask
    {
        public string Url;
        public string Name;
        public string Path;
        public int Size;

        public DownloadTask(string url, string name, string path, int size)
        {
            Url = url;
            Name = name;
            Path = path;
            Size = size;
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
