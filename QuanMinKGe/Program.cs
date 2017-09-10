using Newtonsoft.Json;
using System;
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
            sb.Append(string.Format("Json lib name:{0},version:{1}.", "Newtonsoft.Json", ass2.GetName().Version.ToString()));
            Console.WriteLine(sb.ToString());
        }
        public static  void Main(string[] args)
        {
            DisplayCopyRight();
            string result = null;
            string url="", filename="";
            if (args.Length==2)
            {
                url = args[0];
                filename = args[1];
            }
            else if(args.Length==1)
            {
                url = args[0];
            }
            //else
            //{
            //    Console.WriteLine("参数为地址和保存位置");
            //}
            if (filename!="")
            {
              if (filename==Path.GetFileName(filename))
              {
                filename = Environment.CurrentDirectory + filename;
              }
            }
            
            try
            {
                if (url=="")
                {
                    result = NetConnect.HttpGet("http://node.kg.qq.com/play?s=mS7v0amA7ErFAmOA&g_f=personal").Result;
                }
                else
                {
                    result =NetConnect.HttpGet(url).Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connect Error... \n Message:{0}",e.Message);
            }
            try
            {
                Match m = Regex.Match(result, "window.__DATA__ = (.*?); </script>");
                string mm = m.ToString().Replace("</script>", "").Replace("window.__DATA__ =", "").Replace(";", "");
                Work w = JsonConvert.DeserializeObject<Work>(mm);
                SongInformation.information = w;
                if (w.detail.kg_nick=="☆忘颜乄我在哦")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("这是修改者的全民K歌，欢迎来光临！\n");
                    sb.AppendLine("名称：☆忘颜乄我在哦");
                    sb.AppendFormat("Url:{0}", "http://node.kg.qq.com/personal?uid=6a9b9c8125243f8c");
                    Console.WriteLine(sb.ToString());
                    if (!File.Exists(Environment.CurrentDirectory + "\\Info.txt"))
                    {
                        using (FileStream fs=File.Open(Environment.CurrentDirectory+"\\Info.txt",FileMode.OpenOrCreate,FileAccess.Write))
                        {
                                 using (StreamWriter sw=new StreamWriter(fs))
                                 {
                                    sw.Write(sb.ToString());
                                    sw.Close();
                                  }
                             fs.Close();
                        }
                    }
                  
                }
                Random r = new Random(DateTime.Now.Millisecond);
                if (filename != "")
                {
                    string rpath=NetConnect.HttpDownloadFile(w.detail.playurl, filename).Result;
                }
                string rpath2 = NetConnect.HttpDownloadFile(w.detail.playurl, string.Format(Environment.CurrentDirectory + "\\{0}-{1}-{2}.mp3", w.detail.song_name, w.detail.kg_nick, r.Next())).Result;
                Console.WriteLine("下载完成");
                Console.WriteLine("请按任意键继续…");
                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
