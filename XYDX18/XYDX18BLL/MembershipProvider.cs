using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using XYDX18DAL;



namespace XYDX18BLL
{
    /// <summary>
    /// 自定义Membership类
    /// </summary>
    /// <remarks></remarks>
    public class MembershipProvider
    {
        /// <summary>
        /// 数据库连接实例
        /// </summary>
        xydx18Entities db = new xydx18Entities();

        /// <summary>
        /// 当前上下文访问对象
        /// </summary>
        HttpContext contenxt = HttpContext.Current;
        /// <summary>
        /// 验证用户的合法性
        /// </summary>
        /// <param name="singName">登录名</param>
        /// <param name="password">密码</param>
        /// <param name="outputUser">用户账户信息</param>
        /// <returns>是否登录成功</returns>
        /// <remarks></remarks>
        public bool ValidateUser(string singName, string password, out Account outputUser)
        {
            Account account = IsAuthUser(singName.ToLower(), password);
            if (account != null)
            {
                outputUser = account;
                return true;
            }
            else
            {
                outputUser = null;
                return false;
            }
        }
        /// <summary>
        /// 检查用户是否登录
        /// </summary>
        /// <returns>是否登录</returns>
        /// <remarks></remarks>
        public bool CheckUser()
        {
            if (GetUser() != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得当前登录用户的帐号信息
        /// </summary>
        /// <returns>用户帐户信息</returns>
        /// <remarks></remarks>
        public Account GetUser()
        {
            if (contenxt.User != null && contenxt.User.Identity != null && contenxt.User.Identity.Name != "")
            {
                string accountId = contenxt.User.Identity.Name;
                if (contenxt.Session[accountId] != null)
                    return (contenxt.Session[accountId]) as Account;
                else
                {
                    Account userInfo = Detail(Guid.Parse(accountId));
                    contenxt.Session[accountId] = userInfo;
                    return userInfo;
                }
            }
            else
                return null;
        }
        /// <summary>
        /// 获得登录用户的角色
        /// </summary>
        /// <param name="accountId">帐号ID</param>
        /// <returns>角色集合</returns>
        /// <remarks></remarks>
       /* public string[] GetRoles(string accountId)
        {
            if(accountId!=null && accountId!="")
            {
                List<Role> listRole = GetAccountInRoleList(Guid.Parse(accountId));
                List<string> roleName = new List<string>();
                foreach(Role role in listRole)
                {
                    roleName.Add(role.RoleName);
                }
                return roleName.ToArray();
            }
            return null;
        }*/
     
        /// <summary>
        /// 根据用户名和Mobile获得密码
        /// </summary>
        /// <param name="SignName">用户名</param>
        /// <param name="email">email</param>
        /// <returns>密码</returns>
        /// <remarks></remarks>
        public string GetPassword(string SignName, string email)
        {
            Account userInfo = GetUserInfoByMobile(SignName.ToLower());
            if (userInfo.Mobile == email.ToLower())
            {
                return userInfo.Password;
            }
            return "";
        }
        /// <summary>
        /// 根据登录名获得用户帐户信息
        /// </summary>
        /// <param name="SignName">登录名</param>
        /// <returns>用户帐户信息</returns>
        /// <remarks></remarks>
        public Account FindUsersByName(string SignName)
        {
            return GetUserInfoByMobile(SignName.ToLower());
        }
    

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="SignName">登录名</param>
        /// <param name="oldpwd">旧密码</param>
        /// <param name="newpwd">新密码</param>
        /// <remarks></remarks>
        public void ChangePassword(string SignName, string oldpwd, string newpwd)
        {
            ChangePwd(SignName, oldpwd, newpwd);
        }
        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <remarks></remarks>
        public void LockUser(string userName)
        {
            LockUsers(userName);
        }
        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <remarks></remarks>
        public void UnLockUser(string userName)
        {
            UnlockUsers(userName);
        }
        /// <summary>
        /// 获取出管理员之外的所有的账户信息
        /// </summary>
        /// <returns></returns>
        public List<Account> GetAccountList()
        {
 
            return (from a in db.Account where a.Mobile != "admin" select a).ToList();
        }
        /// <summary>
        /// 添加账户信息
        /// </summary>
        /// <param name="account"></param>
        public void Add(Account account)
        {

            db.Account.Add(account);
            db.SaveChanges();
        }
        #region 自己持有方法

        /// <summary>
        /// 根据账户Id获取账户信息
        /// </summary>
        /// <param name="guid">账户ID</param>
        /// <returns>帐号信息</returns>
        /// <remarks></remarks>
        private Account Detail(Guid guid)
        {
            return (from u in db.Account
                    where u.ID == guid
                    select u).SingleOrDefault();
        }
        public Account DetailByOpenID(string openID)
        {
            return (from u in db.Account
                    where u.openid == openID
                    select u).SingleOrDefault();
        }
        /// <summary>
        /// 根据登录名获得账户信息
        /// </summary>
        /// <param name="UserName">登录名</param>
        /// <returns>账户信息</returns>
        /// <remarks></remarks>
        public Account GetUserInfoByMobile(string Mobile)
        {
            return (from r in db.Account
                    where r.Mobile == Mobile
                    select r).SingleOrDefault();
        }
        /// <summary>
        /// 验证用户合法性
        /// </summary>
        /// <param name="signName">登录名</param>
        /// <param name="Password">密码</param>
        /// <remarks></remarks>
        private Account IsAuthUser(string signName, string Password)
        {
            return (from a in db.Account
                    where a.Mobile == signName && a.Password == Password
                    select a).SingleOrDefault();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <remarks></remarks>
        private void ChangePwd(string userName, string oldPwd, string newPwd)
        {
            Account account = GetUserInfoByMobile(userName.ToLower());
            if (account.Password == oldPwd)
            {
                account.Password = newPwd;
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <remarks></remarks>
        private void LockUsers(string userName)
        {
            Account account = GetUserInfoByMobile(userName.ToLower());
            account.IsLockedOut = true;
            db.SaveChanges();
        }
        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <remarks></remarks>
        private void UnlockUsers(string userName)
        {
            Account account = GetUserInfoByMobile(userName.ToLower());
            account.IsLockedOut = false;
            db.SaveChanges();
        }
        #endregion


        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="account"></param>
        public void Edit()
        {
            db.SaveChanges();
        }
        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Account GetAccountById(Guid id)
        {
            return db.Account.Where(x => x.ID == id).SingleOrDefault();
        }
  
    }
}
