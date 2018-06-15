using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYDX18DAL;
using XYDX18BLL;

namespace XYDX18Website.Controllers
{
    /// <summary>
    /// 系统登录控制器
    /// </summary>
    /// <remarks></remarks>
    public class LoginController : Controller
    {
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="txtUserName">登录名</param>
        /// <param name="txtPwd">密码</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [HttpPost]
        public ActionResult Index(string txtUserName, string txtPwd)
        {
            //自定义Membership类（MembershipProvider——命名空间：System.Web.Security）
            MembershipProvider membership= new MembershipProvider();
            Account userInfo;
            //验证登录 
            //ValidateUser（验证数据源中是否存在指定的用户名和密码）
            //验证成功，则 ValidateUser 更新指定用户的 LastLoginDate(用户上次进行身份验证的日期和时间)
            //对于新用户而言，如果用户尚未登录，则 LastLoginDate 等于 CreationDate。如果成功调用了 ValidateUser 方法，则 LastLoginDate 更新为当前日期和时间
            if (membership.ValidateUser(txtUserName.Trim(), txtPwd.Trim(), out userInfo))
            {
                //授权用户
                //FormsAuthentication（为 Web 应用程序管理 Forms 身份验证服务）
                //SetAuthCookie（为提供的用户名创建一个身份验证票证，并使用提供的 cookie 路径或使用 URL（如果使用的是无 Cookie 身份验证）将该票证添加到响应的 Cookie 集合中）
                //SetAuthCookie(已验证的用户的名称,若要创建持久 Cookie（跨浏览器会话保存的 Cookie）则为 true否则为 false,Forms 身份验证票证的 Cookie 路径)
                System.Web.Security.FormsAuthentication.SetAuthCookie(userInfo.ID.ToString(), true);
                //保存用户信息
                //HttpContext（封装有关个别 HTTP 请求的所有 HTTP 特定的信息）.Session为当前 HTTP 请求获取 HttpSessionState（提供对会话状态值以及会话级别设置和生存期管理方法的访问） 对象
                HttpContext.Session[userInfo.ID.ToString()] = userInfo;

                //设置最后登录时间
                userInfo.LastLoginDate = DateTime.Now;
                membership.Edit();

                if (txtUserName.Trim()=="admin")
                {
                    return RedirectToAction("Index", "SuperAdmin");
                }else
                {
                    return RedirectToAction("ContentsList", "Admin");
                }
            }
            else
            {
                ViewBag.UserName = txtUserName;
                ViewBag.ErrorMSG = "账号或密码错误！";
                return View();
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ActionResult SingOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
