using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using agi = HtmlAgilityPack;

namespace LSFindUser_Web
{
    public partial class search : System.Web.UI.Page
    {
        public static string log = null;
        static WebClient wc = new WebClient();
        static agi.HtmlDocument doc = new agi.HtmlDocument();
        public static string szEncodekey = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string nickName = null;
            nickName = Request.QueryString["nickName"];

            wc.Encoding = Encoding.UTF8;

            bool bResult = FindUser(nickName);
            if (bResult == false)
            {
                log = "NotFound";
            }
            else if (bResult == true)
            {
                GetUserData(szEncodekey);
            }

            Response.Write(log);
        }

        static bool FindUser(string szUserName)
        {
            int iPage = 1, iCount = 0;
            //초기정보 끝

            while (true)
            {
                string szWebAddress = "http://www.lostsaga.com/search/nickname.asp?page=" + iPage + "&sword=" + szUserName;
                string html = wc.DownloadString(szWebAddress);

                for (int i = 0; i < 10; i++)
                {
                    int PlusInt = i + 1;
                    string xPathName = "//*[@id='cont']/div[2]/ul/li[" + PlusInt + "]/a";

                    doc.LoadHtml(html);

                    agi.HtmlNode node = doc.DocumentNode.SelectSingleNode(xPathName);
                    try
                    {
                        iCount++;
                        string ReadData = node.InnerText;

                        if (szUserName == ReadData) //만약 닉네임이 일치 한다면..
                        {
                            string htmlcode = node.Attributes["href"].Value;
                            szEncodekey = htmlcode.Substring(htmlcode.IndexOf('?') + 1).Trim();
                            return true;
                        }
                    }
                    catch
                    {
                        //log = "FindError";
                        return false;
                    }

                }
                iPage++;
            }
        }

        static void GetUserData(string szEncodekey)
        {
            string szSite = "http://www.lostsaga.com/myhomepy/game_info.asp?master=" + szEncodekey;
            string html = wc.DownloadString(szSite);

            doc.LoadHtml(html);

            UserData userinfo = new UserData();

            agi.HtmlNode node_UserName = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[2]/h4/strong");
            userinfo.UserName = node_UserName.InnerText;

            agi.HtmlNode node_Grade = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[4]/div[1]/table/tbody/tr[1]/td[1]");
            userinfo.Grade = node_Grade.InnerText;

            agi.HtmlNode node_SkillGrade = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[4]/div[1]/table/tbody/tr[2]/td[1]");
            userinfo.SkillGrade = node_SkillGrade.InnerText;

            agi.HtmlNode node_Guild = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[4]/div[1]/table/tbody/tr[1]/td[2]");
            try
            {
                userinfo.Guild = node_Guild.InnerText;
            }
            catch
            {
                userinfo.Guild = "길드 없음.";
            }

            agi.HtmlNode node_FinalConnectDay = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[4]/div[1]/table/tbody/tr[3]/td[2]");
            userinfo.FinalConnectDay = node_FinalConnectDay.InnerText;

            agi.HtmlNode node_AccessTime = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[4]/div[1]/table/tbody/tr[4]/td[2]");
            userinfo.AccessTime = node_AccessTime.InnerText;

            agi.HtmlNode node_Ranking = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[3]/div[2]/strong[1]");
            userinfo.Ranking = node_Ranking.InnerText;

            agi.HtmlNode node_Ladder = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[4]/div[4]/div[2]/table/tbody/tr[1]/td");
            userinfo.Ladder = node_Ladder.InnerText;

            agi.HtmlNode node_TodayReader = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[2]/p/strong");
            userinfo.TodayReader = node_TodayReader.InnerText;

            agi.HtmlNode node_TotalReader = doc.DocumentNode.SelectSingleNode("//*[@id='cont']/div[2]/p/span");
            string szTotalReader = node_TotalReader.InnerText;
            userinfo.TotalReader = szTotalReader.Substring(szTotalReader.IndexOf(':') + 1).Trim();


            //Console.WriteLine("닉네임 : " + userinfo.UserName);
            //Console.WriteLine("계급 : " + userinfo.Grade);
            //Console.WriteLine("실력 정보 : " + userinfo.SkillGrade);
            //Console.WriteLine("길드 : " + userinfo.Guild);
            //Console.WriteLine("마지막 접속일 : " + userinfo.FinalConnectDay);
            //Console.WriteLine("누적 접속 시간 : " + userinfo.AccessTime);
            //Console.WriteLine("순위 : " + userinfo.Ranking);
            //Console.WriteLine("래더 순위 : " + userinfo.Ladder);
            //Console.WriteLine("오늘 방문자 수 : " + userinfo.TodayReader);
            //Console.WriteLine("전체 방문자 수 : " + userinfo.TotalReader);
            //Console.WriteLine("마이홈피 주소 : " + szSite);

            log = "닉네임 : " + userinfo.UserName + "\n계급 : " + userinfo.Grade + "\n실력 정보 : " + userinfo.SkillGrade + "\n길드 : " + userinfo.Guild
                + "\n마지막 접속일 : " + userinfo.FinalConnectDay + "\n누적 접속 시간 : " + userinfo.AccessTime + "\n순위 : " + userinfo.Ranking
                + "\n래더 순위 : " + userinfo.Ladder + "\n오늘 방문자 수 : " + userinfo.TodayReader + "\n전체 방문자 수 : " + userinfo.TotalReader;
        }
    }
}