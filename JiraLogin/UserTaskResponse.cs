using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraLogin
{
    public class UserTaskResponse
    {
        public string expand { get; set; }
        public long startAt { get; set; }
        public long maxResults { get; set; }
        public long total { get; set; }
        public List<Issue> issues { get; set; }

    }
    public class Issue
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Dictionary<dynamic,dynamic> fields { get; set; }
    }
}
