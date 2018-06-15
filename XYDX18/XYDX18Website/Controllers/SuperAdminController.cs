using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Data;
using System.Collections;

using XYDX18BLL;
using XYDX18DAL;
using XYDX18Website.SelfModels;
using XYDX18Website;

using eBuul.CodeImage.Core;

namespace XYDX18Website.Controllers
{
    [eBuulAuthorizeAttribute(Roles = "管理员")]
    public class SuperAdminController : Controller
    {
        ContentsBLL contentsBll = new ContentsBLL();
        ContentsLogBLL contentsLogBll = new ContentsLogBLL();
        AccountBLL accountBll = new AccountBLL();
        MembershipProvider membership = new MembershipProvider();

        #region 内容管理
        public ActionResult Index(int? pageNo = 1)
        {
            Account loginAccount = membership.GetUser();
            //未登录
            if (loginAccount == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            else
            {
                if (loginAccount.Mobile != "admin")
                {
                    return RedirectToAction("Index", "Login", null);
                }
            }
            int pageSize = 25;
            ContentsOfPageList liveOfpage = new ContentsOfPageList();
            int total = 0;

            List<Contents> list = contentsBll.GetListByAccountIDForSuper( pageNo.Value, pageSize, out  total);
            liveOfpage.ContentsList = list;
            PagingInfo page = new PagingInfo();
            page.CurrentPage = pageNo.Value;
            page.ItemsPerPage = pageSize;
            page.TotalItems = total;
            liveOfpage.PageInfor = page;

            return View(liveOfpage);
        }

        /// <summary>
        /// 查看内容
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReView(string Id)
        {
            Contents pic = contentsBll.GetContentsById(int.Parse(Id));
            return View(pic);
        }
      

        /// <summary>
        /// 查看二维码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReViewQRCode(string Id)
        {
            ViewBag.ID = Id;
            return View();
        }


        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Cook(string id)
        {
            Cook cook = new Cook();
            //必须先声明类型，默认是二维码
            //cook.Type = Cook.CodeType.CODE39;
            //要的图片的大小(默认200*200)
            cook.Width = 200;
            cook.Height = 200;
            cook.Content = "http://localhost:49158/ReViewQRCode?Id=" + id;

            return File(cook.GetStream(), "image/png");
        }
        #endregion

        #region 日志
        public ActionResult Log(int? pageNo = 1)
        {
            Account loginAccount = membership.GetUser();
            //未登录
            if (loginAccount == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            else
            {
                if (loginAccount.Mobile != "admin")
                {
                    return RedirectToAction("Index", "Login", null);
                }
            }

            int pageSize = 25;
            ContentsLogOfPageList liveOfpage = new ContentsLogOfPageList();
            int total = 0;

            List<ContentsLog> list = contentsLogBll.GetList( pageNo.Value, pageSize, out  total);
            liveOfpage.ContentsLogList = list;
            PagingInfo page = new PagingInfo();
            page.CurrentPage = pageNo.Value;
            page.ItemsPerPage = pageSize;
            page.TotalItems = total;
            liveOfpage.PageInfor = page;

            return View(liveOfpage);
        }
        #endregion


        #region 用户管理
        public ActionResult AccountList(int? pageNo = 1)
        {
            Account loginAccount = membership.GetUser();
            //未登录
            if (loginAccount == null)
            {
                return RedirectToAction("Index", "Login", null);
            }
            else
            {
                if (loginAccount.Mobile != "admin")
                {
                    return RedirectToAction("Index", "Login", null);
                }
            }
            int pageSize = 25;
            AccountOfPageList liveOfpage = new AccountOfPageList();
            int total = 0;

            List<Account> list = accountBll.GetList(pageNo.Value, pageSize, out  total);
            liveOfpage.AccountList = list;
            PagingInfo page = new PagingInfo();
            page.CurrentPage = pageNo.Value;
            page.ItemsPerPage = pageSize;
            page.TotalItems = total;
            liveOfpage.PageInfor = page;

            return View(liveOfpage);
        }

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult AccountAdd(string title, string contents)
        {

            JsonResult js = new JsonResult();

            Account loginAccount = membership.GetUser();
            //未登录
            if (loginAccount == null)
            {
                js.Data = "false";
                return js;
            }

            Account accountModel = new Account();
            accountModel.ID = Guid.NewGuid();
            accountModel.Mobile = title;
            accountModel.Password = contents;
            accountModel.RegDate = DateTime.Now;
            accountBll.AddAccount(accountModel);

            js.Data = "true";
            return js;
        }

        /// <summary>
        /// 编辑账号
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult AccountEdit(string Id, string title, string contents)
        {
            JsonResult js = new JsonResult();

            Account loginAccount = membership.GetUser();
            //未登录
            if (loginAccount == null)
            {
                js.Data = "false";
                return js;
            }
            Account picOld = accountBll.GetAccountById(Guid.Parse(Id));

            picOld.Mobile = title;
            picOld.Password = contents;

            accountBll.EditAccount();

            js.Data = "true";
            return js;
        }
        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [HttpGet]
        public ActionResult AccountDelete(string Id)
        {
            Account loginAccount = membership.GetUser();
            //未登录
            if (loginAccount == null)
            {
                return RedirectToAction("Index", "Login", null);
            }

             accountBll.DeleteAccount(Guid.Parse(Id));

            return RedirectToAction("AccountList");
        }

        #endregion
    }
}
