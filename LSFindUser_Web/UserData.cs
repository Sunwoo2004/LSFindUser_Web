using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSFindUser_Web
{
    public class UserData //웹 파싱
    {
        public string UserName { get; set; }
        public string Grade { get; set; }
        public string SkillGrade { get; set; }
        public string Guild { get; set; }
        public string FinalConnectDay { get; set; }
        public string AccessTime { get; set; }
        public string Ladder { get; set; }
        public string TodayReader { get; set; }
        public string TotalReader { get; set; }
        public string Ranking { get; set; }
    }
}