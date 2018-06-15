using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace XYDX18Website.SelfModels
{
    public class PageExpand<T>
    {

       private PageExpand()
        {
            Collection = new List<T>();
            
        }
       public PageExpand( List<T> entiy,PagingInfo page)
       {
           Collection = entiy;
           Page = page;
       }

       public PagingInfo Page { get; set; }

        public List<T> Collection { get; set; }
    }
}