//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace XYDX18DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        public System.Guid ID { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<bool> IsLockedOut { get; set; }
        public Nullable<int> subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public Nullable<int> sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string subscribe_time { get; set; }
    }
}
