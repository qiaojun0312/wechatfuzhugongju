using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYDX18DAL;

namespace XYDX18BLL
{
    public class AccountBLL
    {
        /// <summary>
        /// 数据库连接实例
        /// </summary>
        xydx18Entities db = new xydx18Entities();

        public List<Account> GetList( int pageNo, int pageSize, out int TotalNumber)
        {
            List<Account> list = (from a in db.Account
                                  select a).OrderByDescending(x => x.RegDate).ToList();
            TotalNumber = list.Count();
            return list.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.RegDate).ToList();
        }

        /// <summary>
        /// 根据Id获得用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户实体</returns>
        /// <remarks></remarks>
        public Account GetAccountById(Guid id)
        {
            return db.Account.Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="Account">用户实体</param>
        /// <remarks></remarks>
        public void AddAccount(Account Account)
        {
            db.Account.Add(Account);
            db.SaveChanges();
        }
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="article">用户实体</param>
        /// <remarks></remarks>
        public void EditAccount()
        {
            db.SaveChanges();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="Id">用户ID</param>
        /// <remarks></remarks>
        public void DeleteAccount(Guid Id)
        {
            db.Account.Remove(GetAccountById(Id));
            db.SaveChanges();
        }
    }
}
