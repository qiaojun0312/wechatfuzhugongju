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
    /// TenpayUtil ��ժҪ˵����
    /// �����ļ�
    /// </summary>
    public class TenPayUtil
    {
        /// <summary>
        /// �������Noncestr
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
        /// ���ַ�������URL����
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
        /// ���ַ�������URL����
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
        /// ȡʱ��������漴��,�滻���׵����еĺ�10λ��ˮ��
        /// </summary>
        /// <returns></returns>
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }
        /// <summary>
        /// ȡ�����
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


        //Get����ʽ
        public string RequestGet(string Url)
        {
            string PageStr = string.Empty;//���ڴ�Ż��ص�html
            Uri url = new Uri(Url);//Uri�� �ṩͳһ��Դ��ʶ�� (URI) �Ķ����ʾ��ʽ�Ͷ� URI �����ֵ����ɷ��ʡ����Ǵ���url��ַ
            try
            {
                HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(url);//����url��ַ����HTTpWebRequest����
                HttpWebResponse response = (HttpWebResponse)httprequest.GetResponse(); ;//ʹ��HttpWebResponse��ȡ����Ļ���ֵ
                Stream steam = response.GetResponseStream();//�ӻ��ض����л�ȡ������
                StreamReader reader = new StreamReader(steam, Encoding.GetEncoding("utf-8"));//��ȡ����Encoding.GetEncoding("gb2312")ָ������gb2312���������Ļ������
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
        /// ����KEY����ȡJson������е�Value
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
                    //�Ƚض��ţ��������һ�����ء������ţ�ȡ��Сֵ
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //�������Ż�ո�
                }
            }
            return result;
        }
    }
}