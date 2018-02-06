using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace QuanMinKGe
{
    public static class SongInformation
    {
        public static Work information;
    }
    public class Program
    {
        static void DisplayCopyRight()
        {
            StringBuilder sb = new StringBuilder("QuanMinKGeDownloader \n");
            Assembly ass = Assembly.GetExecutingAssembly();
            sb.Append(string.Format("This app version:{0}. \n", ass.GetName().Version.ToString()));
            sb.Append("Copyright Q. John & lqj679ssn 2015-2017 \n");
            Assembly ass2 = Assembly.LoadFrom("Newtonsoft.Json.dll");
            sb.Append(string.Format("Json lib name:Newtonsoft.Json,version:{0}.",ass2.GetName().Version.ToString()));
            Console.WriteLine(sb.ToString());
        }
        static bool AssertX(string text,out string resultc)
        {
            string x = "\\[em\\][\\s\\S]*?\\[\\/em\\]";
            Regex regex = new Regex(x);
            bool result = regex.IsMatch(text);
            if (result)
            {
                resultc = Regex.Replace(text, x, "");
            }
            else
            {
                resultc = text;
            }
            return result;
        }
        public static void Main(string[] args)
        {
            DisplayCopyRight();

            string result = null;
            string url = "", filename = "";
            if (args.Length == 2)
            {
                url = args[0];
                filename = args[1];
            }
            else if (args.Length == 1)
            {
                url = args[0];
            }
            else
            {
                Console.WriteLine("参数为地址和保存位置");
            }
            if (url.Substring(0, 10) == "dl.stream.")
            {
                Random r = new Random(DateTime.Now.Millisecond);
                API._isDownloadUrl = true;
                if (filename != "")
                {
                    if (filename == Path.GetFileName(filename))
                    {
                        filename = Environment.CurrentDirectory + filename;
                    }
                    API._PATH = NetConnect.HttpDownloadFile(url, filename).Result;
                }
                else
                {
                    API._PATH = NetConnect.HttpDownloadFile(url, string.Format(Environment.CurrentDirectory + "\\D-{0}.mp3", r.Next())).Result;
                }
                return;
            }
            if (filename != "")
            {
                if (filename == Path.GetFileName(filename))
                {
                    filename = Environment.CurrentDirectory + filename;
                }
            }

            try
            {
                if (url == "")
                {
                    result = NetConnect.HttpGet("http://node.kg.qq.com/play?s=mS7v0amA7ErFAmOA&g_f=personal").Result;
                }
                else
                {
                    result = NetConnect.HttpGet(url).Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connect Error... \n Message:{0}", e.Message);
                API._error.Add(string.Format("Connect Error... \n Message:{0}", e.Message));
            }
            Match m = Regex.Match(result, "window.__DATA__ = (.*?); </script>");
            if (m.Success)
            {
               string mm = m.ToString().Replace("</script>", "").Replace("window.__DATA__ =", "").Replace(";", "");
               Work w = JsonConvert.DeserializeObject<Work>(mm);
               SongInformation.information = w;
               Random r = new Random(DateTime.Now.Millisecond);
                 if (filename != "")
                 {
                     API._PATH = NetConnect.HttpDownloadFile(w.detail.playurl, filename).Result;
                 }
                string temp = w.detail.kg_nick;
                string x = "";
                string result2;
                if (AssertX(temp,out result2))
                {
                    x = result2;
                }
                else
                {
                    x = temp;
                }
                API._PATH = NetConnect.HttpDownloadFile(w.detail.playurl, string.Format(Environment.CurrentDirectory + "\\{0}-{1}-{2}.mp3", w.detail.song_name, x, r.Next())).Result;
                Console.WriteLine("下载完成");
                Console.WriteLine("请按任意键继续…");
            }
            else
            {
                    string message = "您的链接无效或作品已被删除/设置为私密，请重试";
                    Console.WriteLine(message);
                    API._error.Add(message);
            }
            Console.ReadKey();
        }
    }
}
