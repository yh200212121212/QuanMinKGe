using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanMinKGe
{
    public static class API
    {
        internal static string _PATH = "";
        public static string PATH { get { return _PATH; }}
        internal static List<string> _error = new List<string>();
        public static List<string> error { get { return _error; } }
        internal static bool _isDownloadUrl = false;
        public static bool isDownloadUrl { get { return _isDownloadUrl; } }
    }
}
