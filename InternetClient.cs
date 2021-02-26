using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
namespace RequestTest1
{
    class InternetClient
    {
        //連線驗證用帳號
        public static string UserID = string.Empty;
        //連線驗證用密碼
        public static string Password = string.Empty;
        //是否保持連線
        public static bool KeepAlive = false;
        //預設10秒
        public static int Timeout = 10000;
        #region--Http--
        public static string HttpGet(string strUrl)
        {
            return InternetClient.HttpGet(strUrl, System.Text.Encoding.Default);
        }
        public static string HttpGet(string strUrl, Encoding TextEncoding)
        {
            string strRet = "";
            Uri uri = new Uri(strUrl);
            HttpWebRequest hwReq = WebRequest.Create(uri) as HttpWebRequest;
            if ((string.IsNullOrEmpty(UserID) == false) && (string.IsNullOrEmpty(Password) == false))
            {
                hwReq.Credentials = new NetworkCredential(UserID, Password);
            }
            hwReq.Method = WebRequestMethods.Http.Get;
            hwReq.KeepAlive = KeepAlive;
            hwReq.Timeout = Timeout;
            using (HttpWebResponse hwRes = hwReq.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(hwRes.GetResponseStream(), TextEncoding))
                {
                    strRet = reader.ReadToEnd();
                }
            }
            return strRet;
        }

        public static string HttpPost(string strUrl, Dictionary<string, string> postData)
        {
            return InternetClient.HttpPost(strUrl, postData, System.Text.Encoding.Default);
        }
        public static string HttpPost(string strUrl, Dictionary<string, string> postData, Encoding TextEncoding)
        {
            string strRet = "";
            Uri uri = new Uri(strUrl);
            HttpWebRequest hwReq = WebRequest.Create(uri) as HttpWebRequest;
            if ((string.IsNullOrEmpty(UserID) == false) && (string.IsNullOrEmpty(Password) == false))
            {
                hwReq.Credentials = new NetworkCredential(UserID, Password);
            }
            hwReq.Method = WebRequestMethods.Http.Post;
            hwReq.KeepAlive = KeepAlive;
            hwReq.Timeout = Timeout;
            hwReq.ContentType = "application/x-www-form-urlencoded";

            StringBuilder data = new StringBuilder();
            string ampersand = "";
            foreach (string key in postData.Keys)
            {
                data.Append(ampersand).Append(key).Append("=").Append(HttpUtility.UrlEncode(postData[key]));
                ampersand = "&";
            }
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            //設定寫入內容長度
            hwReq.ContentLength = byteData.Length;
            //寫入Post參數
            using (Stream postStream = hwReq.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse hwRes = hwReq.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(hwRes.GetResponseStream(), TextEncoding))
                {
                    strRet = reader.ReadToEnd();
                }
            }
            return strRet;
        }
        #endregion
    }
}
