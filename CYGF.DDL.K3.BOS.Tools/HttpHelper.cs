using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CYSD.DDL.K3.BOS.Tools
{
    public class HttpHelper
    {

        /// <summary>
        /// HttpGet请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        result = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static string Get(string url, Dictionary<string, string> dic)
        {
            string param = GetParam(dic);
            string getUrl = string.Format("{0}?{1}", url, param);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(getUrl);
            req.Method = "GET";
            req.ContentType = "application/json";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            string result = "";
            using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <param name="Cookies">返回的Cookies集合</param>
        /// <returns></returns>
        public static string Get(string url, Dictionary<string, string> dic, ref CookieContainer Cookies)
        {
            string param = GetParam(dic);
            string getUrl = string.Format("{0}?{1}", url, param);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(getUrl);
            req.Method = "GET";
            req.ContentType = "application/json";
            req.CookieContainer = new CookieContainer();
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp.Cookies.Count > 0)
            {
                foreach (Cookie c in (resp.Cookies))
                {
                    Cookies.Add(c);
                }
            }
            string result = "";
            using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        private static string GetParam(Dictionary<string, string> dic)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            return builder.ToString();
        }

        public static string Post(string url, string postData)
        {
            return Post(url, postData, "application/json");
        }
        public static string HttpPost(string url, string postData, string contentType = "application/x-www-form-urlencoded")
        {
            return Post(url, postData, contentType);
        }
        public static string Post(string url, string postData, CookieContainer Cookies)
        {
            return Post(url, postData, "application/json", Cookies);
        }

        /// <summary>
        /// 模拟提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <param name="contentType">application/json</param>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static string HttpPost2(string url, string paramData, string contentType = "application/json", Dictionary<string, string> headerDic = null)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "POST";
                wbRequest.ContentType = contentType;
                wbRequest.ContentLength = Encoding.UTF8.GetByteCount(paramData);
                if (headerDic != null && headerDic.Count > 0)
                {
                    foreach (var item in headerDic)
                    {
                        wbRequest.Headers.Add(item.Key, item.Value);
                    }
                }
                using (Stream requestStream = wbRequest.GetRequestStream())
                {
                    using (StreamWriter swrite = new StreamWriter(requestStream))
                    {
                        swrite.Write(paramData);
                    }
                }
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sread = new StreamReader(responseStream))
                    {
                        result = sread.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            { }

            return result;
        }

        public static string Post(string url, Dictionary<string, string> dic)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            string param = GetParam(dic);
            byte[] data = Encoding.UTF8.GetBytes(param);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            string result = "";
            using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        public static string Post(string url, string postData, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = "POST";
            request.Timeout = 300000;

            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            response.Close();
            return result;
        }



        public static string Post(string url, string postData, string contentType, CookieContainer Cookies)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = "POST";
            request.Timeout = 300000;
            if (Cookies.Count > 0)
            {
                request.CookieContainer = Cookies;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            response.Close();
            return result;
        }


        public static string Post(string url, string postData, string userName, string password)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";

            string usernamePassword = userName + ":" + password;
            CredentialCache credentialCache =
                new CredentialCache { { new Uri(url), "Basic", new NetworkCredential(userName, password) } };
            request.Credentials = credentialCache;
            request.Headers.Add("Authorization",
                "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.ASCII);
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
            string result = reader.ReadToEnd();
            response.Close();
            return result;
        }

        //static CookieContainer cookie = new CookieContainer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="Headers">单据头设置</param>
        /// <returns></returns>
        public static string doHttpPost(string url, string postData, Dictionary<string, string> Headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Timeout = 30000;
            if (Headers != null)
            {
                foreach (var header in Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            response.Close();
            return result;
        }

        /// <summary>
        /// 偶发性超时时试看看
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string HttpPostForTimeOut(string url, string postData)
        {
            //System.Diagnostics.StoSLCKatch watch = new System.Diagnostics.StoSLCKatch();
            //watch.Start();
            GC.Collect();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //int a = Encoding.UTF8.GetByteCount(postData);
            request.Timeout = 20 * 600 * 1000;


            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 200;

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;

            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8")); //如果JSON有中文则是UTF-8
            myStreamWriter.Write(postData);
            myStreamWriter.Close(); //请求中止,是因为长度不够,还没写完就关闭了.

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //watch.Stop();  //停止监视
            //TimeSpan timespan = watch.Elapsed;  //获取当前实例测量得出的总时间
            //System.Diagnostics.Debug.WriteLine("打开窗口代码执行时间：{0}(毫秒)", timespan.TotalMinutes);  //总毫秒数

            Stream myResponseStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResponseStream ?? throw new InvalidOperationException(), Encoding.GetEncoding("utf-8"));
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string registerResult = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return registerResult;
        }
    }
}
