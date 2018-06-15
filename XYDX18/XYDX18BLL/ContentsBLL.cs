using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYDX18DAL;

namespace XYDX18BLL
{
    public class ContentsBLL
    {
        /// <summary>
        /// 数据库连接实例
        /// </summary>
        xydx18Entities db = new xydx18Entities();

        public List<Contents> GetListByAccountID(string accountUser, int pageNo, int pageSize, out int TotalNumber)
        {
               
            List<Contents> list = new List<Contents>();
           
                list = (from a in db.Contents
                                    where a.AccountUser == accountUser && a.IsDelete == 0
                                       select a).OrderByDescending(x => x.AddDate).ToList();
                TotalNumber = list.Count();
                return list.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.AddDate).ToList();
          
        }
        public List<Contents> GetListByAccountIDForSuper( int pageNo, int pageSize, out int TotalNumber)
        {

            List<Contents> list = new List<Contents>();

            list = (from a in db.Contents
                   
                    select a).OrderByDescending(x => x.AddDate).ToList();
            TotalNumber = list.Count();
            return list.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.AddDate).ToList();

        }

        /// <summary>
        /// 根据Id获得内容信息
        /// </summary>
        /// <param name="id">内容ID</param>
        /// <returns>内容实体</returns>
        /// <remarks></remarks>
        public Contents GetContentsById(int id)
        {
            return db.Contents.Where(x => x.ID == id && x.IsDelete==0).SingleOrDefault();
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="Contents">内容实体</param>
        /// <remarks></remarks>
        public void AddContents(Contents Contents)
        {
            db.Contents.Add(Contents);
            db.SaveChanges();
        }
        /// <summary>
        /// 编辑内容
        /// </summary>
        /// <param name="article">内容实体</param>
        /// <remarks></remarks>
        public void EditContents()
        {
            db.SaveChanges();
        }
        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="Id">内容ID</param>
        /// <remarks></remarks>
        public void DeleteContents(int Id)
        {
            db.Contents.Remove(GetContentsById(Id));
            db.SaveChanges();
        }
    }
}
