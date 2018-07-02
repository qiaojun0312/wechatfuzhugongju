using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYDX18BLL;
using XYDX18DAL;

using eBuul.CodeImage.Core;
using XYDX18Website.TenPayLibV3;

namespace XYDX18Website.Controllers
{
    public class ContentsController : Controller
    {
        protected string accessToken = string.Empty;
        public string timeStamp = "", nonceStr = "", strAccess_taken = "", strjsapi_ticket = "", signature = "";

        RequestHandler packageReqHandler = new RequestHandler(null);

        // GET: /Contents/
        ContentsBLL contentsBll = new ContentsBLL();
        public ActionResult Index(string Id)
        {
            timeStamp = TenPayUtil.GetTimestamp();
            nonceStr = TenPayUtil.GetNoncestr();
            //晓峰测试号 appID 和 Scret  appid=wx0bcc58e7cb182a1f&secret=6bca2d2e1e4a0892b462590aec81716f
            //获取全局Access_takenJSON 有效期7200秒，开发者必须在自己的服务全局缓存access_token
            string Access_takenJSON = packageReqHandler.RequestGet("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=xx&secret=xxxx");
            strAccess_taken = packageReqHandler.GetJsonValue(Access_takenJSON, "access_token");


            //获取jsapi_ticket，有效期7200秒，开发者必须在自己的服务全局缓存jsapi_ticket
            string jsapi_ticketJSON = packageReqHandler.RequestGet(string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", strAccess_taken));
            strjsapi_ticket = packageReqHandler.GetJsonValue(jsapi_ticketJSON, "ticket");


            //设置签名参数
            RequestHandler paySignReqHandler = new RequestHandler(null);
            paySignReqHandler.SetParameter("jsapi_ticket", strjsapi_ticket);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("url", Request.Url.AbsoluteUri);
            signature = paySignReqHandler.CreateSHA1Sign();

            ViewBag.TimeStamp = timeStamp;
            ViewBag.NonceStr=nonceStr;
            ViewBag.Signature=signature;

            //ViewBag.JSTickets = strjsapi_ticket;
            //ViewBag.URLS = Request.Url.AbsoluteUri;


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
            cook.Content = "http://xydx18.wcampaign.cn/Contents/Index/?Id=" + id;

            return File(cook.GetStream(), "image/png");
        }

    }
}
