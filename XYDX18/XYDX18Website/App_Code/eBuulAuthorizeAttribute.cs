using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using XYDX18BLL;
using XYDX18DAL;


namespace XYDX18Website
{
    public class eBuulAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 用户等级
        /// </summary>
        private string rating;
        public string Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断用户是否已经登录，没登录跳转到登录页面
            //HttpContext.User为当前 HTTP 请求获取或设置安全信息
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {   
                string okurl = filterContext.HttpContext.Request.RawUrl;
                string redirectUrl = string.Format("?ReturnUrl={0}", okurl);
                string loginUrl = System.Web.Security.FormsAuthentication.LoginUrl + redirectUrl;
                filterContext.Result = new RedirectResult(loginUrl);
            }
            else
            {  //已登录用户
                bool isAuthorize = IsAllowed(base.Roles.Split(','));
                if (!isAuthorize)  //判断用户是否拥有checkRole权限，没有的话跳转到权限错误页。
                    filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { Controller = "Login", Action = "Index" }));
            }
        }
        XYDX18BLL.MembershipProvider mp = new XYDX18BLL.MembershipProvider();
        /// <summary>
        /// 是否运行指定的角色访问
        /// </summary>
        /// <param name="roleIn"></param>
        /// <returns></returns>
        public bool IsAllowed(string[] roleIn)
        {

            if (roleIn.Contains("管理员"))
            {
                return true;
            }
            return false;
        }
    }
}