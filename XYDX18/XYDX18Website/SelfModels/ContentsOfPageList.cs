using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYDX18DAL;

namespace XYDX18Website.SelfModels
{
    public class ContentsOfPageList
    {
        private List<Contents> contentsList;

        public List<Contents> ContentsList
        {
            get { return contentsList; }
            set { contentsList = value; }
        }
        private PagingInfo pageInfor;

        public PagingInfo PageInfor
        {
            get { return pageInfor; }
            set { pageInfor = value; }
        }
    }
}