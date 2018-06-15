using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using XYDX18Website.SelfModels;


namespace XYDX18Website.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinkes(this HtmlHelper html,bool ischina, string controller, string ActionName, PagingInfo pagingInfo, string[] paramArray)
        {
           
            HttpContext contenxt = HttpContext.Current;
            if (pagingInfo.TotalPages <= 1)
            {
                return MvcHtmlString.Create("");
            }
            //设置分页显示的区间值
            int IsNextPage = 1;
            int currentPage = (pagingInfo.CurrentPage - 1) / 5;
            int start = 1, end = 5;
            if (currentPage >= 1)
            {
                start = (currentPage * 5) + 1;
                end = (currentPage + 1) * 5;
            }
            StringBuilder result = new StringBuilder();
            string tmp = "";
            if (paramArray != null && paramArray.Length > 0)
            {
                for (int n = 0; n < paramArray.Length; n++)
                {
                    if (contenxt.Request.Params.AllKeys.Contains(paramArray[n]))
                        tmp += "&" + paramArray[n] + "=" + contenxt.Request.Params[paramArray[n]];
                }
            }

            //生成中间的分页数目
            int i = 0;
            for (i = start; i <= end; i++)
            {
                if (i <= pagingInfo.TotalPages)
                {
                    TagBuilder litag = new TagBuilder("li");
                    TagBuilder tag = new TagBuilder("a");
                    if (tmp != "")
                        tag.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}{3}", controller, ActionName, i, tmp));
                    else
                        tag.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}", controller, ActionName, i));
                    tag.InnerHtml = i.ToString();
                    if (i == pagingInfo.CurrentPage)
                        litag.AddCssClass("hover");
                    litag.InnerHtml = tag.ToString();
                    result.Append(litag.ToString());
                    IsNextPage++;
                }
                else
                    break;
            }
            //设置分页显示的下一区间值的起始位置(即页面中的"..."按钮)
            if (pagingInfo.TotalPages > 5 && (i - 1) != pagingInfo.TotalPages)
            {
                TagBuilder litagDot = new TagBuilder("li");
                TagBuilder tagDot = new TagBuilder("a");
                if (tmp != "")
                    tagDot.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}{3}", controller, ActionName, end + 1, tmp));
                else
                    tagDot.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}", controller, ActionName, end + 1));
                tagDot.InnerHtml = "...";
                litagDot.InnerHtml = tagDot.ToString();
                result.Append(litagDot.ToString());
            }
            //设置分页显示的上一区间值的起始位置(即页面中的"..."按钮)
            if (currentPage >= 1)
            {
                TagBuilder litagDot = new TagBuilder("li");
                TagBuilder tagDot = new TagBuilder("a");
                if (tmp != "")
                    tagDot.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}{3}", controller, ActionName, start - 1, tmp));
                else
                    tagDot.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}", controller, ActionName, start - 1));
                tagDot.InnerHtml = "...";
                litagDot.InnerHtml = tagDot.ToString();
                result.Insert(0, litagDot.ToString());
            }

            //上一页
            if (pagingInfo.TotalPages > 1)
            {
                TagBuilder litagPre = new TagBuilder("li");
                TagBuilder tagPre = new TagBuilder("a");
                //TagBuilder tagImgPre = new TagBuilder("img");
                //tagImgPre.MergeAttribute("src", "/Content/imghome/left02.png");

                if (pagingInfo.CurrentPage == 1)
                    tagPre.MergeAttribute("href", "#");
                else
                {
                    if (tmp != "")
                        tagPre.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}{3}", controller, ActionName, pagingInfo.CurrentPage - 1, tmp));
                    else
                        tagPre.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}", controller, ActionName, pagingInfo.CurrentPage - 1));
                }
                //tagPre.InnerHtml = tagImgPre.ToString();
                if (!ischina)
                {
                    tagPre.InnerHtml = "Previous";
                }
                else
                {
                    tagPre.InnerHtml = "上一页";
                }
                litagPre.InnerHtml = tagPre.ToString();
                result.Insert(0, litagPre);
            }
            //下一页
            if (pagingInfo.TotalPages > 1)
            {
                TagBuilder litagNext = new TagBuilder("li");
                TagBuilder tagNext = new TagBuilder("a");
                //TagBuilder tagImgNext = new TagBuilder("img");
                //tagImgNext.MergeAttribute("src", "/Content/imghome/right02.png");
                if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
                    tagNext.MergeAttribute("href", "#");
                else
                {
                    if (tmp != "")
                        tagNext.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}{3}", controller, ActionName, pagingInfo.CurrentPage + 1, tmp));
                    else
                        tagNext.MergeAttribute("href", string.Format("/{0}/{1}?pageNo={2}", controller, ActionName, pagingInfo.CurrentPage + 1));
                }
                //tagNext.InnerHtml = tagImgNext.ToString();
                if (!ischina)
                {
                    tagNext.InnerHtml = "Next";
                }
                else
                {
                    tagNext.InnerHtml = "下一页";
                }
                litagNext.InnerHtml = tagNext.ToString();
                result.Append(litagNext.ToString());
            }
            //总页数
            TagBuilder litagTotal = new TagBuilder("li");
            TagBuilder spantag = new TagBuilder("span");
            litagTotal.AddCssClass("li_none");
            spantag.InnerHtml = pagingInfo.TotalItems.ToString();
            if (!ischina)
            {
                litagTotal.InnerHtml = "A total of " + spantag.ToString() + " records";
            }
            else
            {
                litagTotal.InnerHtml = "共" + spantag.ToString() + "条记录";
            }
            result.Append(litagTotal.ToString());

            TagBuilder ultag = new TagBuilder("ul");
            ultag.AddCssClass("dc_pagination dc_paginationA dc_paginationA06");
            ultag.InnerHtml = result.ToString();
            string strsss= ultag.ToString();

            return MvcHtmlString.Create(strsss);
        }
    }
}