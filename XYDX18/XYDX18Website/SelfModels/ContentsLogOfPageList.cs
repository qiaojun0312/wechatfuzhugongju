using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYDX18DAL;

namespace XYDX18Website.SelfModels
{
    public class ContentsLogOfPageList
    {
        private List<ContentsLog> contentsLogList;

        public List<ContentsLog> ContentsLogList
        {
            get { return contentsLogList; }
            set { contentsLogList = value; }
        }
        private PagingInfo pageInfor;

        public PagingInfo PageInfor
        {
            get { return pageInfor; }
            set { pageInfor = value; }
        }
    }
}