using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYDX18Website.SelfModels
{
    public class PagingInfo
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }
    }
}