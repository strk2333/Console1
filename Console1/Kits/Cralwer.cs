using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;

namespace Kits
{
    class Cralwer
    {
        static private string _root;
        private string file;
        private Stack<string> postUrls;
        private static List<string[]> nameDatabase1;
        private static List<string[]> nameDatabase2;
        private List<TieBaUser> users;
        private int passCount = 0;
        //private List<string> files;

        public Cralwer(string root)
        {
            nameDatabase1 = new List<string[]>();
            nameDatabase2 = new List<string[]>();
            postUrls = new Stack<string>();
            users = new List<TieBaUser>();
            //tmpCookie = new CookieContainer();
            //tmpCookie.SetCookies(new Uri(_root), "BDUSS=p2WU5FS3ZKRnZTc3FFajlwYVo3MmV0OUt0eVpmU1NMU2FYaFRjd1pqaWt0cHBaTVFBQUFBJCQAAAAAAAAAAAEAAAA718I2SVdOc3VyAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKQpc1mkKXNZQU; BAIDUID=FECF9A4ED9BAF4D73006337F31483E88:FG=1; PSTM=1498011163; BIDUPSID=FECF9A4ED9BAF4D73006337F31483E88; MCITY=-%3A; PSINO=3; H_PS_PSSID=1461_21092_18560_17001; BDSFRCVID=TIAsJeC62ReD6hcZtZPxhdrQ9g5Yr4bTH6ao6KCu7MiNxMSLFblaEG0PqU8g0KubaaNdogKKL2OTHmoP;");

            _root = root;
        }
        public Cralwer() : this(string.Format("http://tieba.baidu.com/f?ie=utf-8&kw={0}&fr=search", "Switch"))
        {
        }

        public void Start()
        {
            HandlePagesPostCrawl(100);

            //foreach (string s in CrawlRemarkData(@"https://tieba.baidu.com/p/5279372068"))
            //{
            //    Console.WriteLine(s);
            //}

            //GetPostUrls(_root);
            //postUrls.Push(_root);
            //HandlePagesRemarkCrawl();
        }

        private void DataHandles()
        {
            Print(nameDatabase1, @"C:\Users\1\Documents\TieBaData.txt");
            //ShowDatabaseCMD();
            PackData(nameDatabase1);
            users.Sort();
            Print(users, @"C:\Users\1\Documents\TieBaUsersPostData.txt");
            //ShowUsersCMD();
        }

        #region Crawl
        public void CrawlTrial()
        {
            HttpWebRequest req = WebRequest.Create(_root) as HttpWebRequest;
            req.Method = "GET";
            file = GetContent(req.GetResponse().GetResponseStream());
            //<li class=" j_thread_list clearfix" data-field="
            //string[] result = Analyst(file, "<li class=\" j_thread_list clearfix\" data-field=\"");
            string[] result = Analyst(file, @"<li class="" j_thread_list[\s\S]+?</li>");
            string[] houser = GetHouser(result);
            Console.WriteLine(result.Length);

            foreach (string s in houser)
            {
                Console.WriteLine("-" + s);
            }
            Console.WriteLine("---------------------------------");
        }

        Stack<string> urlStack = new Stack<string>();
        int threadCount;
        public void HandlePagesPostCrawl(int count)
        {
            int num = count;
            threadCount = count > 10 ? 10 : count;
            Thread[] threads = new Thread[threadCount];
            object sync = new object();

            for (int i = 0; i < count; i++)
            {
                urlStack.Push(_root + "&pn=" + i * 50);
            }

            for (int i = 0; i < threadCount; i++)
            {
                string tmp;
                int j = i;
                threads[i] = CreateCrawlThread(j);
                //threads[i] = new Thread(() =>
                //{
                //    while (urls.Count != 0)
                //    {
                //        tmp = urls.Pop();
                //        nameDatabase1.Add(CrawlPosterData(tmp));
                //        Console.WriteLine(++passCount + "pass");
                //    }
                //    //lock (sync)
                //    {
                //        Console.WriteLine(j + "Done");

                //        if (--threadCount == 0)
                //        {
                //            Console.WriteLine("All Done");

                //            DataHandles();
                //            Console.WriteLine("UserData Done");
                //        }
                //    }
                //});
            }

            foreach (Thread t in threads)
                t.Start();
        }

        private Thread CreateCrawlThread(int index)
        {
            string tmp;
            return new Thread(() =>
            {
                while (urlStack.Count != 0)
                {
                    tmp = urlStack.Pop();
                    nameDatabase1.Add(CrawlPosterData(tmp));
                    Console.WriteLine(++passCount + "pass");
                }
                //lock (sync)
                {
                    Console.WriteLine(index + "Done");

                    if (--threadCount == 0)
                    {
                        Console.WriteLine("All Done");

                        DataHandles();
                        Console.WriteLine("UserData Done");
                    }
                }
            }); 
        }

        public void HandlePagesRemarkCrawl()
        {
            int num = postUrls.Count;
            int threadCount = num > 5 ? 5 : num;
            Thread[] threads = new Thread[threadCount];
            object sync = new object();

            for (int i = 0; i < threadCount; i++)
            {
                int j = i;
                threads[j] = new Thread(() =>
                {
                    nameDatabase2.Add(CrawlRemarkData(postUrls.Pop()));

                    lock (sync)
                    {
                        Console.WriteLine("Done");
                        if (--num == 0)
                        {
                            Console.WriteLine("All Remark Done");

                            PackData(nameDatabase2);
                            Print(nameDatabase2, @"C:\Users\1\Documents\TieBaUsersRemarkData.txt");
                            Console.WriteLine("UserData Remark Done");
                        }
                    }
                });
            }

            foreach (Thread t in threads)
                t.Start();
        }

        public void GetPostUrls(string rootUrl)
        {
            HttpWebRequest req = WebRequest.Create(rootUrl) as HttpWebRequest;
            req.Method = "GET";
            string tmp = GetContent(req.GetResponse().GetResponseStream());
            string[] tmp2 = Analyst(tmp, @"<a href=""/p/[\s\S]+?</a>");
            string[] tmp3 = GetHouser(tmp2);
            Thread.Sleep(1000);
            foreach (string s in tmp3)
            {
                postUrls.Push(s);
            }
        }

        CookieContainer tmpCookie;
        private string[] CrawlPosterData(string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse res = null;
            HttpStatusCode status = HttpStatusCode.Forbidden;
            req.Method = "POST";
            req.Referer = "https://www.baidu.com";
            string tmp;
            string[] tmp2;
            string[] tmp3;
            if (tmpCookie == null)
            {
                req.CookieContainer = new CookieContainer();
                tmpCookie = req.CookieContainer;
            }
            else
            {
                req.CookieContainer = tmpCookie;
            }
            req.CookieContainer = tmpCookie;
            //req.Accept = "text/html, application/xhtml+xml, */*";
            //req.ContentType = "application/x-www-form-urlencoded";

            if (status == HttpStatusCode.Forbidden)
            {
                try
                {
                    status = (res = req.GetResponse() as HttpWebResponse).StatusCode;
                }
                catch (System.Net.WebException e)
                {
                    Console.WriteLine("HTTP Status: 403");
                    Console.WriteLine("URL:" + url);
                    urlStack.Push(url);
                    Thread.Sleep(1000);
                    Console.WriteLine("Try again");
                    CreateCrawlThread(0).Start();
                    Thread.CurrentThread.Abort();
                }
            }
            //tmpCookie.Add(res.Cookies);

            tmp = GetContent(res.GetResponseStream());
            tmp2 = Analyst(tmp, @"<li class="" j_thread_list[\s\S]+?</li>");
            tmp3 = GetHouser(tmp2);
            Thread.Sleep(200);
            return tmp3;
        }

        private static string[] CrawlRemarkData(string url)
        {
            // <div class="l_post l_post_bright j_l_post clearfix  " 
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "GET";
            string tmp = GetContent(req.GetResponse().GetResponseStream());
            string[] tmp2 = Analyst(tmp, @"<a data-field=[\s\S]+?</a>");
            string[] tmp3 = GetRemarker(tmp2);
            Thread.Sleep(200);

            return tmp3;
        }

        private static string[] CrawlPostUrlData(string url)
        {
            // <div class="l_post l_post_bright j_l_post clearfix  " 
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "GET";
            string tmp = GetContent(req.GetResponse().GetResponseStream());
            string[] tmp2 = Analyst(tmp, @"<a data-field=[\s\S]+?</a>");
            string[] tmp3 = GetRemarker(tmp2);
            Thread.Sleep(200);

            return tmp3;
        }

        private void GetPostUrls(string[] target)
        {
            foreach (string s in target)
                GetPostUrls(s);
        }

        private static string[] GetRemarker(string[] target)
        {
            // 
            List<string> list = new List<string>();
            string tmp;
            foreach (string t in target)
            {
                tmp = Regex.Match(t, @"target=""_blank"">[^<]*<").Value;
                if (tmp.Length != 0)
                {
                    tmp = tmp.Remove(0, @"target=""_blank"">".Length);
                    tmp = tmp.Remove(tmp.Length - 1, 1);
                    list.Add(tmp);
                }
            }

            return list.ToArray();
        }

        private static string[] GetHouser(string[] target)
        {
            // "author_name":"\u5982\u7159\u4e36Saki"
            List<string> list = new List<string>();
            string tmp;
            foreach (string t in target)
            {
                tmp = Regex.Match(t, @"""主题作者: [^""]*""").Value;
                if (tmp.Length != 0)
                {
                    tmp = tmp.Remove(0, @"""主题作者: ".Length);
                    tmp = tmp.Remove(tmp.Length - 1, 1);
                    list.Add(tmp);
                }
            }

            return list.ToArray();
        }

        private static string[] Analyst(string target, string header)
        {
            var tmp = Regex.Matches(target, header).GetEnumerator();
            List<string> list = new List<string>();
            while (tmp.MoveNext())
            {
                list.Add(((Match)tmp.Current).Value);
            }

            return list.ToArray();
        }

        private static string GetContent(Stream s)
        {
            StreamReader sr = new StreamReader(s, System.Text.Encoding.UTF8);
            string file = "";
            while (!sr.EndOfStream)
            {
                file += sr.ReadLine();
            }
            sr.Close();
            return file;
        }
        #endregion

        #region Pack Data
        private void PackData(List<string[]> database)
        {
            Dictionary<string, int> data = new Dictionary<string, int>();
            foreach (string[] ss in database)
            {
                foreach (string s in ss)
                {
                    if (data.Keys.Contains(s))
                        ++data[s];
                    else
                        data.Add(s, 1);
                }
            }

            foreach (var i in data)
                users.Add(new TieBaUser(i.Key, i.Value));
        }

        #endregion

        #region DataSave & Print
        private void Print(Stream s)
        {
            StreamReader sr = new StreamReader(s, System.Text.Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                Console.WriteLine(sr.ReadLine());
            }
            sr.Close();
        }

        private void Print(Stream s, string path)
        {
            StreamReader sr = new StreamReader(s, System.Text.Encoding.UTF8);
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                sw.WriteLine(sr.ReadLine());
            }
            sr.Close();
            sw.Close();
        }

        private static void Print(List<string[]> list, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                    sw.WriteLine(list[i][j]);
                sw.WriteLine("----------------------------------------");
            }
            sw.Close();
        }

        private static void Print(List<TieBaUser> users, string path)
        {
            int c = 0;
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            foreach (var user in users)
            {
                sw.WriteLine(user.name + "-" + user.postCount);
            }
            sw.Close();
        }

        private void ShowDatabaseCMD()
        {
            for (int i = 0; i < nameDatabase1.Count; i++)
            {
                foreach (string s in nameDatabase1[i])
                {
                    Console.WriteLine("-" + s);
                }
                Console.WriteLine("----------------------------------");
            }
        }

        private void ShowUsersCMD()
        {
            foreach (var n in users)
            {
                Console.WriteLine(n.name + "-" + n.postCount);
            }
        }

        #endregion
    }
}
