using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
namespace XYDX18Website.TenPayLibV3
{
    /// <summary>
    /// TenpayUtil 的摘要说明。
    /// 配置文件
    /// </summary>
    public class TenPayUtil
    {
        /// <summary>
        /// 随机生成Noncestr
        /// </summary>
        /// <returns></returns>
        public static string GetNoncestr()
        {
            Random random = new Random();
            return MD5Util.GetMD5(random.Next(1000).ToString(), "GBK");
        }

        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 对字符串进行URL编码
        /// </summary>
        /// <param name="instr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string UrlEncode(string instr, string charset)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = System.Web.HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = System.Web.HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;
            }
        }

        /// <summary>
        /// 对字符串进行URL解码
        /// </summary>
        /// <param name="instr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string UrlDecode(string instr, string charset)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = System.Web.HttpUtility.UrlDecode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = System.Web.HttpUtility.UrlDecode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;

            }
        }


        /// <summary>
        /// 取时间戳生成随即数,替换交易单号中的后10位流水号
        /// </summary>
        /// <returns></returns>
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }
        /// <summary>
        /// 取随机数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string BuildRandomStr(int length)
        {
            Random rand = new Random();

            int num = rand.Next();

            string str = num.ToString();

            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str.Insert(0, "0");
                    n--;
                }
            }

            return str;
        }


        //Get请求方式
        public string RequestGet(string Url)
        {
            string PageStr = string.Empty;//用于存放还回的html
            Uri url = new Uri(Url);//Uri类 提供统一资源标识符 (URI) 的对象表示形式和对 URI 各部分的轻松访问。就是处理url地址
            try
            {
                HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(url);//根据url地址创建HTTpWebRequest对象
                HttpWebResponse response = (HttpWebResponse)httprequest.GetResponse(); ;//使用HttpWebResponse获取请求的还回值
                Stream steam = response.GetResponseStream();//从还回对象中获取数据流
                StreamReader reader = new StreamReader(steam, Encoding.GetEncoding("utf-8"));//读取数据Encoding.GetEncoding("gb2312")指编码是gb2312，不让中文会乱码的
                PageStr = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception e)
            {
                PageStr += e.Message;
            }
            return PageStr;
        }
        /// <summary>
        /// 根据KEY，获取Json结果集中的Value
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }
    }
}