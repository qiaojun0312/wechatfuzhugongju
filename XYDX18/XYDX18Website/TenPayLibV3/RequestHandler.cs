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
using System.Web.Security;

namespace XYDX18Website.TenPayLibV3
{
    /**
    '签名工具类
     ============================================================================/// <summary>
    'api说明：
    'Init();
    '初始化函数，默认给一些参数赋值。
    'SetKey(key_)'设置商户密钥
    'CreateMd5Sign(signParams);字典生成Md5签名
    'GenPackage(packageParams);获取package包
    'CreateSHA1Sign(signParams);创建签名SHA1
    'ParseXML();输出xml
    'GetDebugInfo(),获取debug信息
     * 
     * ============================================================================
     */
    public class RequestHandler
    {

        public RequestHandler(HttpContext httpContext)
        {
            Parameters = new Hashtable();

            this.HttpContext = httpContext ?? HttpContext.Current;

        }
        /// <summary>
        /// 密钥
        /// </summary>
        private string Key;

        protected HttpContext HttpContext;

        /// <summary>
        /// 请求的参数
        /// </summary>
        protected Hashtable Parameters;

        /// <summary>
        /// debug信息
        /// </summary>
        private string DebugInfo;

        /// <summary>
        /// 初始化函数
        /// </summary>
        public virtual void Init()
        {
        }
        /// <summary>
        /// 获取debug信息
        /// </summary>
        /// <returns></returns>
        public String GetDebugInfo()
        {
            return DebugInfo;
        }
        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return Key;
        }
        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key"></param>
        public void SetKey(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void SetParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (Parameters.Contains(parameter))
                {
                    Parameters.Remove(parameter);
                }

                Parameters.Add(parameter, parameterValue);
            }
        }


        /// <summary>
        /// 创建md5摘要,规则是:按参数名称a-z排序,遇到空值的参数不参加签名
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// key和value通常用于填充最后一组参数
        /// <returns></returns>
        public virtual string CreateMd5Sign(string key, string value)
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append(key + "=" + value);
            string sign = MD5Util.GetMD5(sb.ToString(), GetCharset()).ToUpper();

            return sign;
        }


        /// <summary>
        /// 创建package签名
        /// </summary>
        /// <returns></returns>
        public virtual string CreateMd5Sign()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "".CompareTo(v) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }
            string sign = MD5Util.GetMD5(sb.ToString(), GetCharset()).ToUpper();

            return sign;
        }
        public string CreateSHA1Sign()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();
            string str="";
            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "".CompareTo(v) != 0 && "sign".CompareTo(k) != 0)
                {
                    sb.Append(k.ToLower() + "=" + v + "&");
                }
            }
             str=sb.ToString();

            if (str.EndsWith("&"))
            {
                str = str.Substring(0, str.Length - 1);
            }
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            return sign;
        }
        /// <summary>
        /// 输出XML
        /// </summary>
        /// <returns></returns>
        public string ParseXML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (string k in Parameters.Keys)
            {
                string v = (string)Parameters[k];
                if (Regex.IsMatch(v, @"^[0-9.]$"))
                {

                    sb.Append("<" + k + ">" + v + "</" + k + ">");
                }
                else
                {
                    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                }

            }
            sb.Append("</xml>");
            return sb.ToString();
        }



        /// <summary>
        /// 设置debug信息
        /// </summary>
        /// <param name="debugInfo"></param>
        public void SetDebugInfo(String debugInfo)
        {
            this.DebugInfo = debugInfo;
        }

        public Hashtable GetAllParameters()
        {
            return this.Parameters;
        }

        protected virtual string GetCharset()
        {
            return this.HttpContext.Request.ContentEncoding.BodyName;
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
