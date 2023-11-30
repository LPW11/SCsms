## 主要的项目结构

<img src="https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129122656986.png" alt="image-20231129122656986" style="zoom:67%;" />

### 如何创建

![image-20231123193958222](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231123193958222.png)

### 关于MVC

![这里写图片描述](https://img-blog.csdn.net/20161121194824256)

### Controllers 层

![image-20231129123335319](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129123335319.png)

```c#
//LoginController
namespace SCsms.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        //数据库连接上下文对象
        private SCsmsEntities db = new SCsmsEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(String username, String password,String loginType)
        {
            if (String.IsNullOrEmpty(username))
            {
                //拿到赋值给到viewbag然后来到前端显示
                ViewBag.notice = "用户名不为空!";
                return View();
            }
            if (String.IsNullOrEmpty(password))
            {
                ViewBag.notice = "密码不为空!";
                return View();
            }

            Users user = db.Users.FirstOrDefault(p => p.UserName == username);
            if (user == null)
            {
                ViewBag.notice = "用户不存在!";
            }
            else if (user.Password != password)
            {
                ViewBag.notice = "密码不正确!";
            }
            //user.UserType 是一个字符串，而 loginType 是从前端传递过来的字符串值。在比较两者是否相等时，你应该使用字符串比较而不是引用比较。可以使用 String.Equals 方法进行比较
            else if (!String.Equals(user.UserType, loginType, StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.notice = "用户类型不正确!";
            }
            else
            {
                //登录成功-直接跳转到对应控制器下的视图
                
                if(String.Equals("Admin", loginType, StringComparison.OrdinalIgnoreCase))
                    return Redirect("/Admin/Index");
                else if(String.Equals("Normal", loginType, StringComparison.OrdinalIgnoreCase))
                    return Redirect("/User/Index");

            }

            return View();
        }

        //测试查询结果

    }
}
```



### Models 层

#### 关于创建

![image-20231123194041259](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231123194041259.png)

![image-20231129124316279](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129124316279.png)

### Views 层

![image-20231129124549112](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129124549112.png)

![image-20231129123722569](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129123722569.png)

## 关于一些逻辑问题

### 登录跳转

拿到数据进行比较，然后根据不同情况进行跳转

![20231129130232 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129130232%2000_00_00-00_00_30.gif)

![image-20231129125108775](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129125108775.png)

发送的地方（form 表单提交）

![image-20231129125331337](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129125331337.png)

### 关于侧边导航栏的制作

![20231129131058 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129131058%2000_00_00-00_00_30.gif)

布局页面的制作

![image-20231129131625879](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129131625879.png)

![image-20231129131957072](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129131957072.png)

![image-20231129132014225](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129132014225.png)

### 查询页面的制作

![20231129134448 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129134448%2000_00_00-00_00_30.gif)

#### 简单展示

```c#
public ActionResult EventsIndex()
{
    List<Events> Event = db.Events.ToList();
    return View(Event);
}
```

![image-20231129133100993](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129133100993.png)

#### 复杂展示（查询）

```c#
//跨表展示，而且要根据原表数据生成新的字段，因此要进行join 连接，在AdminController里面
public class AthleteViewModel
{
    public int AthleteID { get; set; }
    public int EventID { get; set; }
    public string UserName { get; set; }
    public string Sex { get; set; }
    public string EventName { get; set; }
    // 添加其他需要显示的属性
    public TimeSpan Pre_Result { get;
    public string Pre_Sta { get; set; }
    public TimeSpan Fin_Result { get; set; }
    
    public string Fin_Ranking { get; set; }
}

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
    
//根据条件筛选
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
                    Pre_Result = ac.Pre_Result != null ? (TimeSpan)ac.Pre_Result : TimeSpan.Zero,
                    Fin_Result = ac.Fin_Result != null ? (TimeSpan)ac.Fin_Result : TimeSpan.Zero,
                    Pre_Sta = GetPreStaText(ac.AthleteID, ac.EventID, ac.Pre_Sta),
                    Fin_Ranking = GetFinRankingText(ac.Fin_Result, ac.EventID)
                })
                .ToList();

            return View(athletesViewModel);
        }
```

参数哪里来的——表单提交的时候传过来的

![image-20231129134403589](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129134403589.png)

### 信息添加

![20231129140534 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129140534%2000_00_00-00_00_30.gif)

![20231129135233 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129135233%2000_00_00-00_00_30.gif)

![20231129135511 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129135511%2000_00_00-00_00_30.gif)

![20231129135854 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129135854%2000_00_00-00_00_30.gif)

```c#
//AdminController
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
```

![image-20231129140253178](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129140253178.png)

### 修改与删除

![20231129140534 00_00_00-00_00_30](https://lpw11.oss-cn-beijing.aliyuncs.com/img/20231129140534%2000_00_00-00_00_30.gif)

```c#
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
                // 如果找不到对应的信息，处理错误或重定向到合适的页面
                return Content("<script>alert('未找到运动员编号！');window.location.href= '/Admin/Athletes_UD';</script>");
            }

            return View(Athlete);
        }

        //点击修改按钮——传了值
        [HttpPost]
        public ActionResult UpdateIndex(Athletes updatedAthlete)
        {
            var existingAthlete = db.Athletes.Find(updatedAthlete.AthleteID);

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
```

![image-20231129141133879](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129141133879.png)

![image-20231129141102361](https://lpw11.oss-cn-beijing.aliyuncs.com/img/image-20231129141102361.png)
