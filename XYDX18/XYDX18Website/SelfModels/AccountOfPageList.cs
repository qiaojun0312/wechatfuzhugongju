using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYDX18DAL;

namespace XYDX18Website.SelfModels
{
    public class AccountOfPageList
    {
        private List<Account> accountList;

        public List<Account> AccountList
        {
            get { return accountList; }
            set { accountList = value; }
        }
        private PagingInfo pageInfor;

        public PagingInfo PageInfor
        {
            get { return pageInfor; }
            set { pageInfor = value; }
        }
    }
}