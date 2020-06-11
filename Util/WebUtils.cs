using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace WeiXin.Util
{

    internal static class WebUtils
    {
        /// <summary>
        /// 发起支付请求,,微信支付调用GetHtml时响应信息乱码，需要调用本方法
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="url"></param>
        /// <param name="isUseCert">是否启用证书</param>
        /// <param name="certPwd">使用证书 必填证书密码</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="timeout">单位为秒</param>
        /// <returns></returns>
        public static string PayPost(string xml, string url, bool isUseCert, string certPwd, string certPath, out string msg, int timeout = 10)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            msg = null;
            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    X509Certificate2 cert = new X509Certificate2(certPath, certPwd);
                    request.ClientCertificates.Add(cert);
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (WebException e)
            {
                var sr = new StreamReader(e.Response.GetResponseStream(), Encoding.UTF8);
                var tt = sr.ReadToEnd().Trim();
                msg = tt;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 附有重试逻辑的请求Html
        /// </summary>
        /// <param name="hi"></param>
        /// <param name="maxTryCount"></param>
        /// <returns></returns>
        public static HttpResult GetHtml(HttpItem hi, int maxTryCount = 4)
        {
            var hr = new HttpHelper().GetHtml(hi);

            var errMsg = hr.StatusDescription == null ? "" : hr.StatusDescription.ToString();
            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                maxTryCount--;
                if (maxTryCount > 0)
                {
                    return GetHtml(hi, maxTryCount);
                }
                if (maxTryCount <= 1)
                {
                    //Log.ErrorFormat("请求失败，代理：{0} url:{1} msg:{2}", ip, hi.URL, errMsg);
                }
            }
            return hr;
        }

        /// <summary>
        /// 组装GET请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>带参数的GET请求URL</returns>
        public static string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }
            return url;
        }
        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            var postData = new StringBuilder();
            bool hasParam = false;

            if (parameters != null)
            {
                IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
                while (dem.MoveNext())
                {
                    string name = dem.Current.Key;
                    string value = dem.Current.Value;
                    // 忽略参数名为空的参数
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (hasParam)
                        {
                            postData.Append("&");
                        }

                        postData.Append(name);
                        postData.Append("=");
                        postData.Append(HttpUtility.UrlEncode(value ?? "", Encoding.UTF8));
                        hasParam = true;
                    }
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 将&串联的参数列表拆分为字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IDictionary<string, string> SplitUrlQuery(string query)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            string[] pairs = query.Split(new char[] { '&' });
            if (pairs != null && pairs.Length > 0)
            {
                foreach (string pair in pairs)
                {
                    string[] oneParam = pair.Split(new char[] { '=' }, 2);
                    if (oneParam != null && oneParam.Length == 2)
                    {
                        result.Add(oneParam[0], oneParam[1]);
                    }
                }
            }

            return result;
        }

    }
}
