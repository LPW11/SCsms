using SCsms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCsms.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public SCsmsEntities db = new SCsmsEntities();
        public ActionResult Index()
        {
            return View();
        }

        public class AthleteViewModel
        {
            public int AthleteID { get; set; }
            public int EventID { get; set; }
            public string UserName { get; set; }
            public string Sex { get; set; }
            public string EventName { get; set; }
            // 添加其他需要显示的属性
            public TimeSpan Pre_Result { get; set; }

            public string Pre_Sta { get; set; }
            public TimeSpan Fin_Result { get; set; }

            public string Fin_Ranking { get; set; }
        }

        public class filteredAthletes
        {
            public int AthleteID { get; set; }
            public int EventID { get; set; }
            public string UserName { get; set; }
            public string Sex { get; set; }
            public string EventName { get; set; }
            // 添加其他需要显示的属性
            public TimeSpan Pre_Result { get; set; }

            public string Pre_Sta { get; set; }
            public TimeSpan Fin_Result { get; set; }

            public string Fin_Ranking { get; set; }
        }

        //运动员展示页面


        public ActionResult AthletesIndex(string athleteName, string eventName)
        {
            var athletesWithEvents = (from a in db.Athletes
                                      join e in db.Events on a.EventID equals e.EventID
                                      join c in db.Results on new { AthleteID = (int)a.AthleteID, EventID = (int)e.EventID } equals new { AthleteID = (int)c.AthleteID, EventID = (int)c.EventID }
                                      orderby c.Pre_Time // 根据 Pre_Time 排序
                                      select new
                                      {
                                          AthleteID = a.AthleteID,
                                          EventID = (int)a.EventID,
                                          UserName = a.UserName,
                                          Sex = a.Sex,
                                          EventName = e.EventName,
                                          Pre_Result = c.Pre_Time,
                                          Fin_Result = c.Fin_Time,
                                          Pre_Sta = c.Pre_Time
                                      })
                                    .ToList();

            List<AthleteViewModel> athletesViewModel;

            if (string.IsNullOrEmpty(athleteName) && string.IsNullOrEmpty(eventName))
            {
                // 如果运动员姓名和赛事名称都为空，则显示所有数据
                athletesViewModel = athletesWithEvents
                    .Select(ac => new AthleteViewModel
                    {
                        AthleteID = ac.AthleteID,
                        EventID = ac.EventID,
                        UserName = ac.UserName,
                        Sex = ac.Sex,
                        EventName = ac.EventName,
                        Pre_Result = ac.Pre_Result != null ? (TimeSpan)ac.Pre_Result : TimeSpan.Zero,
                        Fin_Result = ac.Fin_Result != null ? (TimeSpan)ac.Fin_Result : TimeSpan.Zero,
                        Pre_Sta = GetPreStaText(ac.AthleteID, ac.EventID, ac.Pre_Sta),
                        Fin_Ranking = GetFinRankingText(ac.Fin_Result, ac.EventID)
                    })
                    .ToList();
            }
            else
            {
                // 否则，根据用户输入筛选运动员信息
                var filteredAthletes = athletesWithEvents.Where(a =>
                    (string.IsNullOrEmpty(athleteName) || a.UserName.Contains(athleteName)) &&
                    (string.IsNullOrEmpty(eventName) || a.EventName.Contains(eventName))
                ).ToList();

                athletesViewModel = filteredAthletes
                    .Select(ac => new AthleteViewModel
                    {
                        AthleteID = ac.AthleteID,
                        EventID = ac.EventID,
                        UserName = ac.UserName,
                        Sex = ac.Sex,
                        EventName = ac.EventName,
                        Pre_Result = ac.Pre_Result != null ? (TimeSpan)ac.Pre_Result : TimeSpan.Zero,
                        Fin_Result = ac.Fin_Result != null ? (TimeSpan)ac.Fin_Result : TimeSpan.Zero,
                        Pre_Sta = GetPreStaText(ac.AthleteID, ac.EventID, ac.Pre_Sta),
                        Fin_Ranking = GetFinRankingText(ac.Fin_Result, ac.EventID)
                    })
                    .ToList();
            }

            return View(athletesViewModel);
        }






        // 辅助方法：根据时间判断 Pre_Sta 文本
        private string GetPreStaText(int athleteID, int eventID, TimeSpan? preTime)
        {
            if (preTime.HasValue)
            {

                var topEightMinTime = db.Results
                    .Where(r => r.EventID == eventID && r.Pre_Time.HasValue)
                    .OrderBy(r => r.Pre_Time)
                    .Take(8)
                    .Max(r => r.Pre_Time);


                return preTime <= topEightMinTime ? "晋级决赛" : "淘汰";
            }
            else
            {
                return string.Empty; // 如果没有 Pre_Result 的时间，返回空字符串
            }
        }

        // 辅助方法：根据 Fin_Result 时间判断 Fin_Ranking 文本
        private string GetFinRankingText(TimeSpan? finTime, int eventID)
        {
            if (finTime.HasValue)
            {
                // 获取所有有 Fin_Result 时间的运动员，并按 Fin_Result 时间升序排序
                var rankedAthletes = db.Results
                    .Where(r => r.EventID == eventID && r.Fin_Time.HasValue)
                    .OrderBy(r => r.Fin_Time)
                    .ToList();

                // 查找当前运动员在排序后的列表中的位置，然后添加相应的字符
                var ranking = rankedAthletes.FindIndex(r => r.Fin_Time == finTime) + 1;

                return $"第{ranking}名";
            }
            else
            {
                return string.Empty; // 如果没有 Fin_Result 的时间，返回空字符串
            }
        }

        [HttpPost]
        public ActionResult AthletesIndex(string athleteName, string eventName, string test)
        {
            var athletesWithEvents = (from a in db.Athletes
                                      join e in db.Events on a.EventID equals e.EventID
                                      join c in db.Results on new { AthleteID = (int)a.AthleteID, EventID = (int)e.EventID } equals new { AthleteID = (int)c.AthleteID, EventID = (int)c.EventID }
                                      orderby c.Pre_Time // 根据 Pre_Time 排序
                                      select new
                                      {
                                          AthleteID = a.AthleteID,
                                          EventID = (int)a.EventID,
                                          UserName = a.UserName,
                                          Sex = a.Sex,
                                          EventName = e.EventName,
                                          Pre_Result = c.Pre_Time,
                                          Fin_Result = c.Fin_Time,
                                          Pre_Sta = c.Pre_Time
                                      })
                                    .ToList();

            // 根据用户输入筛选运动员信息
            var filteredAthletes = athletesWithEvents.Where(a =>
                (string.IsNullOrEmpty(athleteName) || a.UserName.Contains(athleteName)) &&
                (string.IsNullOrEmpty(eventName) || a.EventName.Contains(eventName))
            ).ToList();

            var athletesViewModel = filteredAthletes
                .Select(ac => new AthleteViewModel
                {
                    AthleteID = ac.AthleteID,
                    EventID = ac.EventID,
                    UserName = ac.UserName,
                    Sex = ac.Sex,
                    EventName = ac.EventName,
                    Pre_Result = (TimeSpan)ac.Pre_Result,
                    Fin_Result = (TimeSpan)ac.Fin_Result,
                    Pre_Sta = GetPreStaText(ac.AthleteID, ac.EventID, ac.Pre_Sta),
                    Fin_Ranking = GetFinRankingText(ac.Fin_Result, ac.EventID)
                })
                .ToList();

            return View(athletesViewModel);
        }


        public ActionResult EventsIndex()
        {
            List<Events> Event = db.Events.ToList();
            return View(Event);
        }

        public ActionResult Athletes_reg()
        {
            List<Athletes> Athlete = db.Athletes.ToList();
            return View(Athlete);
        }
    }
}