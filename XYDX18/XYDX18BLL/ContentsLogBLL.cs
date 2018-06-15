using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYDX18DAL;

namespace XYDX18BLL
{
    public class ContentsLogBLL
    {
        /// <summary>
        /// 数据库连接实例
        /// </summary>
        xydx18Entities db = new xydx18Entities();

        public List<ContentsLog> GetList( int pageNo, int pageSize, out int TotalNumber)
        {
            List<ContentsLog> list = (from a in db.ContentsLog
                                      select a).OrderByDescending(x => x.AddDate).ToList();
            TotalNumber = list.Count();
            return list.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.AddDate).ToList();
        }

        public List<ContentsLog> GetList(string AccountUser, int pageNo, int pageSize, out int TotalNumber)
        {
            List<ContentsLog> list = (from a in db.ContentsLog
                                      where a.AccountUser == AccountUser
                                   select a).OrderByDescending(x => x.AddDate).ToList();
            TotalNumber = list.Count();
            return list.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.AddDate).ToList();
        }


        /// <summary>
        /// 根据Id获得内容日志信息
        /// </summary>
        /// <param name="id">内容日志ID</param>
        /// <returns>内容日志实体</returns>
        /// <remarks></remarks>
        public ContentsLog GetContentsLogById(int id)
        {
            return db.ContentsLog.Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// 添加内容日志
        /// </summary>
        /// <param name="ContentsLog">内容日志实体</param>
        /// <remarks></remarks>
        public void AddContentsLog(ContentsLog ContentsLog)
        {
            db.ContentsLog.Add(ContentsLog);
            db.SaveChanges();
        }
        /// <summary>
        /// 编辑内容日志
        /// </summary>
        /// <param name="article">内容日志实体</param>
        /// <remarks></remarks>
        public void EditContentsLog()
        {
            db.SaveChanges();
        }
        /// <summary>
        /// 删除内容日志
        /// </summary>
        /// <param name="Id">内容日志ID</param>
        /// <remarks></remarks>
        public void DeleteContentsLog(int Id)
        {
            db.ContentsLog.Remove(GetContentsLogById(Id));
            db.SaveChanges();
        }
    }
}
