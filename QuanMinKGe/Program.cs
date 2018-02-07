using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using PauseHelpeerLib;
namespace QuanMinKGe
{
    public static class SongInformation
    {
        public static Work Information;
    }
    public class Program
    {
        static void DisplayCopyRight()
        {
            StringBuilder sb = new StringBuilder("App name:QuanMinKGeDownloader \n");
            Assembly ass = Assembly.GetExecutingAssembly();
            sb.Append(string.Format("App version:{0}. \n", ass.GetName().Version.ToString()));
            sb.Append("Copyright Q. John & lqj679ssn 2015-2017 \n");
            Assembly ass2 = Assembly.LoadFrom("Newtonsoft.Json.dll");
            sb.Append(string.Format("Json lib name:Newtonsoft.Json,version:{0}",ass2.GetName().Version.ToString()));
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
                API._IsDownloadUrl = true;
                if (filename != "")
                {
                    if (filename == Path.GetFileName(filename))
                    {
                        filename = Environment.CurrentDirectory + filename;
                    }
                    API._Path = NetConnect.HttpDownloadFile(url, filename);
                }
                else
                {
                    API._Path = NetConnect.HttpDownloadFile(url, string.Format(Environment.CurrentDirectory + "\\D-{0}.m4a", r.Next()));
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
                    result = NetConnect.HttpGet("http://node.kg.qq.com/play?s=mS7v0amA7ErFAmOA&g_f=personal");
                }
                else
                {
                    result = NetConnect.HttpGet(url);
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
               string ResultJson = m.ToString().Replace("</script>", "").Replace("window.__DATA__ =", "").Replace(";", "");
               Work w = JsonConvert.DeserializeObject<Work>(ResultJson);
               SongInformation.Information = w;
               Random random = new Random(DateTime.Now.Millisecond);
               if (filename != "")
               {
                 API._Path = NetConnect.HttpDownloadFile(w.detail.playurl, filename);
               }
                string temp = w.detail.kg_nick;
                string nick2;
                API._Path = NetConnect.HttpDownloadFile(w.detail.playurl, string.Format(Environment.CurrentDirectory + "\\{0}-{1}-{2}.m4a", w.detail.song_name, AssertX(temp, out nick2)?nick2:temp, random.Next()));
                Console.WriteLine("下载完成");
            }
            else
            {
                    string message = "您的链接无效或作品已被删除/设置为私密，请重试";
                    Console.WriteLine(message);
                    API._error.Add(message);
            }
            Helper.Invoke();
        }
    }
}
