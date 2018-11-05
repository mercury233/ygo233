using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.IO;

namespace YGO233
{
    public static class Downloader
    {
        private static List<DownloadTask> tasks = new List<DownloadTask>();
        private static bool allFinished = false;

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
            new FileInfo(path).Directory.Create();
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

        public static void DownloadFileAsync(DownloadTask task, Func<DownloadTask, int> callback, Func<DownloadTask, Exception, int> fail)
        {
            new FileInfo(task.Path).Directory.Create();
            task.Started = true;
            YGO233WebClient client = new YGO233WebClient();
            client.DownloadFileCompleted += (sender, e) =>
            {
                task.Finished = true;
                if (e.Error != null)
                    fail(task, e.Error);
                else
                    callback(task);
            };
            client.DownloadFileAsync(new Uri(task.Url), task.Path);
        }

        public static void ClearTasks()
        {
            tasks.Clear();
            allFinished = false;
        }

        public static void AddTask(string url, string name, string path)
        {
            tasks.Add(new DownloadTask(url, name, path));
            allFinished = false;
        }

        public static void AddTask(DownloadTask task)
        {
            tasks.Add(task);
            allFinished = false;
        }

        public static void ProcressDownload(Func<int, int, string, int> one, Func<int, int> finish)
        {
            if (!allFinished && tasks.All(task => task.Finished))
            {
                allFinished = true;
                finish(tasks.Count);
                return;
            }
            int downloadingCount = tasks.Count(task => task.Started && !task.Finished);
            if (downloadingCount < 5)
            {
                var newTasks = tasks.Where(task => !task.Started && !task.Finished).Take(5 - downloadingCount).ToList();
                newTasks.ForEach(task=> {
                    DownloadFileAsync(task, (_task)=> { one(tasks.Count, tasks.Count(__task=>__task.Finished), task.Name); ProcressDownload(one, finish); return 0; }, (_task, _) => { one(tasks.Count, tasks.Count(__task => __task.Finished), task.Name); ProcressDownload(one, finish); return 0; });
                });
            }
        }
    }

    public class DownloadTask
    {
        public string Url;
        public string Name;
        public string Path;
        public bool Started;
        public bool Finished;

        public DownloadTask(string url, string name, string path)
        {
            Url = url;
            Name = name;
            Path = path;
            Started = false;
            Finished = false;
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
