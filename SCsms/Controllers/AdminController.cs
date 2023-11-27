using SCsms.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SCsms.Controllers
{
    public class AdminController : Controller
    {
        public SCsmsEntities db = new SCsmsEntities();

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


        // GET: Admin
        public ActionResult Index()
        {
            return View();
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
        public ActionResult AthletesIndex(string athleteName, string eventName,string test)
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

        public ActionResult EventsAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EventsAdd(Events Event)
        {
            ViewBag.notice = "";
            db.Events.Add(Event);
            //执行完添加后还要加一句话,执行修改数据库操作
            int res = db.SaveChanges();
            if (res > 0)
            {
                //保存成功，跳回到查询页
                return Content("<script>alert('保存比赛信息成功！');window.location.href= '/Admin/EventsIndex';</script>");
            }
            else
            {
                ViewBag.notice = "保存比赛信息，请重试";
            }
            return View();
        }

        public ActionResult AthletesAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AthletesAdd(Athletes Athlete)
        {
            ViewBag.notice = "";
            db.Athletes.Add(Athlete);
            //执行完添加后还要加一句话,执行修改数据库操作
            int res = db.SaveChanges();
            if (res > 0)
            {
                //保存成功，跳回到查询页
                return Content("<script>alert('保存运动员成功！');window.location.href= '/Admin/AthletesIndex';</script>");
            }
            else
            {
                ViewBag.notice = "保存运动员失败，请重试";
            }
            return View();
        }

        public ActionResult ResultsAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResultsAdd(Results Result)
        {
            ViewBag.notice = "";
            db.Results.Add(Result);
            //执行完添加后还要加一句话,执行修改数据库操作
            int res = db.SaveChanges();
            if (res > 0)
            {
                //保存成功，跳回到查询页
                return Content("<script>alert('保存成绩成功！');window.location.href= '/Admin/AthletesIndex';</script>");
            }
            else
            {
                ViewBag.notice = "保存成绩失败，请重试";
            }
            return View();
        }

        public ActionResult Athletes_reg()
        {
            List<Athletes> Athlete = db.Athletes.ToList();
            return View(Athlete);
        }
        //一张列表+按钮
        public ActionResult Athletes_UD()
        {
            List<Athletes> Athlete = db.Athletes.ToList();
            return View(Athlete);
        }
        //找到单条信息——进行修改
        public ActionResult AthletesUpdate(int AthleteID)
        {
            if (AthleteID == default(int))
            {
                // 如果图书编号为空，处理错误或重定向到合适的页面
                return RedirectToAction("/Admin/Athletes_UD");
            }
            //把要修改的信息传过来（取回来）
            Athletes Athlete = db.Athletes.FirstOrDefault(b => b.AthleteID == AthleteID);

            if (Athlete == null)
            {
                // 如果找不到对应的图书信息，处理错误或重定向到合适的页面
                return Content("<script>alert('未找到运动员编号！');window.location.href= '/Admin/Athletes_UD';</script>");
            }

            return View(Athlete);
        }

        //点击修改按钮——传了值
        [HttpPost]
        public ActionResult UpdateIndex(Athletes updatedAthlete)
        {
            // 获取要更新的书籍
            var existingAthlete = db.Athletes.Find(updatedAthlete.AthleteID);

            // 检查书籍是否存在
            if (existingAthlete == null)
            {
                ViewBag.notice = "编辑运动员失败，该运动员可能已被删除，请刷新页面获取最新数据。";
            }

            // 更新实体的属性
            existingAthlete.AthleteID = updatedAthlete.AthleteID; // 替换 Property1 和其他属性为你的实体属性
            existingAthlete.EventID = updatedAthlete.EventID;
            existingAthlete.UserName = updatedAthlete.UserName;
            existingAthlete.Sex = updatedAthlete.Sex;
            // ...


            // 标记实体状态为已修改
            db.Entry(existingAthlete).State = EntityState.Modified;

            // 保存更改
            int res = db.SaveChanges();

            if (res > 0)
            {
                // 保存成功，执行相关操作
                return Content("<script>alert('编辑运动员成功！');window.location.href= '/Admin/Athletes_reg';</script>");
            }
            else
            {
                ViewBag.notice = "编辑运动员失败，请重试";
            }


            return View(updatedAthlete);
        }

        public ActionResult Events_UD()
        {
            List<Events> Event = db.Events.ToList();
            return View(Event);
        }

        public ActionResult EventsUpdate(int EventID)
        {
            if (EventID == default(int))
            {
                // 如果图书编号为空，处理错误或重定向到合适的页面
                return RedirectToAction("/Admin/Events_UD");
            }
            //把要修改的信息传过来（取回来）
            Events Event = db.Events.FirstOrDefault(b => b.EventID == EventID);

            if (Event == null)
            {
                // 如果找不到对应的图书信息，处理错误或重定向到合适的页面
                return Content("<script>alert('未找到比赛编号！');window.location.href= '/Admin/Events_UD';</script>");
            }

            return View(Event);
        }

        [HttpPost]
        public ActionResult EventsUpdate(Events updatedEvent)
        {
            // 获取要更新的书籍
            var existingEvent = db.Events.Find(updatedEvent.EventID);

            // 检查书籍是否存在
            if (existingEvent == null)
            {
                ViewBag.notice = "编辑运动员失败，该运动员可能已被删除，请刷新页面获取最新数据。";
            }

            // 更新实体的属性
            existingEvent.EventID = updatedEvent.EventID; // 替换 Property1 和其他属性为你的实体属性
            existingEvent.EventName = updatedEvent.EventName;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.MaxParticipants = updatedEvent.MaxParticipants;
            existingEvent.EventGroup = updatedEvent.EventGroup;
            // ...


            // 标记实体状态为已修改
            db.Entry(existingEvent).State = EntityState.Modified;

            // 保存更改
            int res = db.SaveChanges();

            if (res > 0)
            {
                // 保存成功，执行相关操作
                return Content("<script>alert('编辑比赛成功！');window.location.href= '/Admin/EventsIndex';</script>");
            }
            else
            {
                ViewBag.notice = "编辑比赛失败，请重试";
            }


            return View(updatedEvent);
        }

        public ActionResult AthletesDelete(int AthleteID)
        {
            using (var context = new SCsmsEntities())
            {
                // 查询要删除的实体
                var AthleteToDelete = context.Athletes.Find(AthleteID);

                if (AthleteToDelete != null)
                {
                    // 从上下文中移除实体
                    context.Athletes.Remove(AthleteToDelete);

                    // 保存更改到数据库
                    context.SaveChanges();

                    // 可以在这里添加一些成功删除后的处理逻辑
                    return Content("<script>alert('删除运动员成功！');window.location.href= '/Admin/Athletes_reg';</script>");
                }
                else
                {
                    // 实体不存在，可以添加一些相应的处理逻辑
                    ViewBag.Notice = "要删除的记录不存在。";
                }
            }

            // 可以在这里添加其他逻辑或跳转到其他视图
            return View();
        }

        public ActionResult EventsDelete(int EventID)
        {
            using (var context = new SCsmsEntities())
            {
                // 查询要删除的实体
                var EventToDelete = context.Events.Find(EventID);

                if (EventToDelete != null)
                {
                    // 从上下文中移除实体
                    context.Events.Remove(EventToDelete);

                    // 保存更改到数据库
                    context.SaveChanges();

                    // 可以在这里添加一些成功删除后的处理逻辑
                    return Content("<script>alert('删除比赛成功！');window.location.href= '/Admin/EventsIndex';</script>");
                }
                else
                {
                    // 实体不存在，可以添加一些相应的处理逻辑
                    ViewBag.Notice = "要删除的记录不存在。";
                }
            }

            // 可以在这里添加其他逻辑或跳转到其他视图
            return View();
        }

        //一张表+按钮
        public ActionResult Account_inq()
        {
            List<Users> User = db.Users.ToList();
            return View(User);
        }


        public ActionResult AccountUpdate(int UserID)
        {
            if (UserID == default(int))
            {
                // 如果图书编号为空，处理错误或重定向到合适的页面
                return RedirectToAction("/Admin/Account_inq");
            }
            //把要修改的信息传过来（取回来）
            Users user = db.Users.FirstOrDefault(b => b.UserID == UserID);

            if (user == null)
            {
                // 如果找不到对应的图书信息，处理错误或重定向到合适的页面
                return Content("<script>alert('未找到账号编号！');window.location.href= '/Admin/Account_inq';</script>");
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult AccountUpdate(Users updatedAccount)
        {
            // 获取要更新的书籍
            var existingAccount = db.Users.Find(updatedAccount.UserID);

            // 检查书籍是否存在
            if (existingAccount == null)
            {
                ViewBag.notice = "编辑运动员失败，该运动员可能已被删除，请刷新页面获取最新数据。";
                return Content("<script>window.location.href= '/Admin/Account_inq';</script>");
            }

            // 更新实体的属性
            existingAccount.UserID = updatedAccount.UserID; // 替换 Property1 和其他属性为你的实体属性
            existingAccount.UserName = updatedAccount.UserName;
            existingAccount.Password = updatedAccount.Password;
            existingAccount.UserType = updatedAccount.UserType;
            // ...


            // 标记实体状态为已修改
            db.Entry(existingAccount).State = EntityState.Modified;

            // 保存更改
            int res = db.SaveChanges();

            if (res > 0)
            {
                // 保存成功，执行相关操作
                return Content("<script>alert('编辑账号成功！');window.location.href= '/Admin/Account_inq';</script>");
            }
            else
            {
                ViewBag.notice = "编辑账号失败，请重试";
            }


            return View(updatedAccount);
        }

        public ActionResult AccountsAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AccountsAdd(Users User)
        {
            ViewBag.notice = "";
            db.Users.Add(User);
            //执行完添加后还要加一句话,执行修改数据库操作
            int res = db.SaveChanges();
            if (res > 0)
            {
                //保存成功，跳回到查询页
                return Content("<script>alert('添加用户成功！');window.location.href= '/Admin/Account_inq';</script>");
            }
            else
            {
                ViewBag.notice = "添加用户失败，请重试";
            }
            return View();
        }


    }
}