using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace QuanMinKGe
{
    public class Work
    {
        public string shareid;
        public Detail detail;
        public bool isMV;
    }
    public class Detail
    {
        public string avatar;
        public int comment_num;
        public List<Comment> comments;
        public string content;
        public string cover;
        public string ctime;
        public int flower_num;
        public List<Flower> flower;
        public int gift_num;
        public int play_num;
        public string playurl;
        public string playurl_video;
        public int score;
        public string singer_name;
        public string song_name;
        public string tail_name;
        public string uid;
        public string kg_nick;
    }
    public class Comment
    {
        public string avatar;
        public string comment_id;
        public string comment;
        public long ctime;
        public int is_owner;
        public string nick;
        public string reply_avatar;
        public string reply_nick;
        public string uid;
    }
    public class Flower
    {
        public string avatar;
        public string nick;
        public int num;
        public int type;
        public string uid;
    }
}
