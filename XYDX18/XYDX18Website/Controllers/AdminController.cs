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
    public partial class AdminController : Controller
    {
        ContentsBLL contentsBll = new ContentsBLL();
        ContentsLogBLL contentsLogBll = new ContentsLogBLL();
        MembershipProvider membership = new MembershipProvider();

         #region 内容管理
         public ActionResult ContentsList(int? pageNo = 1)
         {
             Account loginAccount = membership.GetUser();
             //未登录
             if (loginAccount == null)
             {
                 return RedirectToAction("Index","Login",null);
             }

             int pageSize = 25;
             ContentsOfPageList liveOfpage = new ContentsOfPageList();
             int total = 0;

             List<Contents> list = contentsBll.GetListByAccountID(loginAccount.Mobile, pageNo.Value, pageSize, out  total);
             liveOfpage.ContentsList = list;
             PagingInfo page = new PagingInfo();
             page.CurrentPage = pageNo.Value;
             page.ItemsPerPage = pageSize;
             page.TotalItems = total;
             liveOfpage.PageInfor = page;

             return View(liveOfpage);
         }
   
         /// <summary>
         /// 添加内容
         /// </summary>
         /// <returns></returns>
         /// 
         [HttpPost]
         public JsonResult ContentsAdd(string title,string contents)
         {

             JsonResult js = new JsonResult();

             Account loginAccount = membership.GetUser();
             //未登录
             if (loginAccount == null)
             {
                 js.Data = "false";
                 return js;
             }

             Contents contentModel = new Contents();
             contentModel.Title = title;
             contentModel.XContents = contents;
             contentModel.AddDate = DateTime.Now;
             contentModel.IsDelete = 0;
             contentModel.AccountUser = loginAccount.Mobile;
             contentsBll.AddContents(contentModel);

             //日志
             ContentsLog logModel = new ContentsLog();
             logModel.OpType = "新增";
             logModel.ContentsID = contentModel.ID;
             logModel.BfTitleUpdate = title;
             logModel.BfContentsUpdate = contents;
             logModel.AddDate = DateTime.Now;
             logModel.AccountUser = loginAccount.Mobile;
             contentsLogBll.AddContentsLog(logModel);

             js.Data = "true";
             return js;
         }

         /// <summary>
         /// 编辑内容
         /// </summary>
         /// <returns></returns>
         /// 
         [HttpPost]
         public JsonResult ContentsEdit(string Id, string title, string contents)
         {
             JsonResult js = new JsonResult();

             Account loginAccount = membership.GetUser();
             //未登录
             if (loginAccount == null)
             {
                 js.Data = "false";
                 return js;
             }
             string oldTitles="";
             string oldContents = "";

             Contents picOld = contentsBll.GetContentsById(int.Parse(Id));
             oldTitles = picOld.Title;
             oldContents = picOld.XContents;
             picOld.Title = title;
             picOld.XContents = contents;

             contentsBll.EditContents();

             //日志
             ContentsLog logModel = new ContentsLog();
             logModel.OpType = "修改";
             logModel.ContentsID = int.Parse(Id);
             logModel.BfTitleUpdate = oldTitles;
             logModel.BfContentsUpdate = oldContents;
             logModel.ATitleUpdate = title;
             logModel.AfContentsUpdate = contents;
             logModel.AddDate = DateTime.Now;
             logModel.AccountUser = loginAccount.Mobile;
             contentsLogBll.AddContentsLog(logModel);

          
             js.Data = "true";
             return js;
         }
         /// <summary>
         /// 删除内容
         /// </summary>
         /// <param name="Id">The id.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         [HttpGet]
         public ActionResult ContentsDelete(string Id)
         {
             Account loginAccount = membership.GetUser();
             //未登录
             if (loginAccount == null)
             {
                 return RedirectToAction("Index","Login",null);
             }

             Contents picOld = contentsBll.GetContentsById(int.Parse(Id));
             picOld.IsDelete = 1;
             contentsBll.EditContents();

             ContentsLog logModel = new ContentsLog();
             logModel.OpType = "删除";
             logModel.ContentsID = int.Parse(Id);
             logModel.BfTitleUpdate = picOld.Title;
             logModel.BfContentsUpdate = picOld.XContents;
             logModel.AddDate = DateTime.Now;
             logModel.AccountUser = loginAccount.Mobile;
             contentsLogBll.AddContentsLog(logModel);

             return RedirectToAction("ContentsList");
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
         #endregion

       
    }
}
