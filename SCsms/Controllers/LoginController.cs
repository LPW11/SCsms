using SCsms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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