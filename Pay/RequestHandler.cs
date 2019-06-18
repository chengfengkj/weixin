using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WeiXin.Util;

namespace WeiXin.Pay
{
    /// <summary>
    /// 支付请求拼装处理器
    /// </summary>
    public class RequestHandler
    {
        public RequestHandler()
        {
            Parameters = new Hashtable();

            HttpContext = new DefaultHttpContext();
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
        /// 当参数不为null或空字符串时，设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void SetParameterWhenNotNull(string parameter, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameterValue))
            {
                SetParameter(parameter, parameterValue);
            }
        }

        /// <summary>
        /// 创建md5摘要,规则是:按参数名称a-z排序,遇到空值的参数不参加签名
        /// <para>key和value通常用于填充最后一组参数</para>
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="workPaySignType">企业支付签名（workwx_sign）类型，默认为 None</param>
        /// <returns></returns>
        public virtual string CreateMd5Sign(string key, string value)
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort(ASCIISort.Create());

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0
                    //&& "sign_type".CompareTo(k) != 0
                    && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append(key + "=" + value);

            //string sign = EncryptHelper.GetMD5(sb.ToString(), GetCharset()).ToUpper();

            //编码强制使用UTF8：https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_1
            string sign = EncryptHelper.GetMD5(sb.ToString(), "UTF-8").ToUpper();

            return sign;
        }

        //public virtual string CreateMd5SignForWork(string corpPaySecret,string )

        /// <summary>
        /// 创建sha256摘要,规则是:按参数名称a-z排序,遇到空值的参数不参加签名
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// key和value通常用于填充最后一组参数
        /// <returns></returns>
        public virtual string CreateSha256Sign(string key, string value)
        {
            var sb = new StringBuilder();

            var akeys = new ArrayList(Parameters.Keys);
            akeys.Sort(ASCIISort.Create());

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0
                    //&& "sign_type".CompareTo(k) != 0
                    && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append(key + "=" + value);

            //string sign = EncryptHelper.GetMD5(sb.ToString(), GetCharset()).ToUpper();

            //编码强制使用UTF8：https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_1

            // 现支持HMAC-SHA256签名方式（EncryptHelper.GetHmacSha256方法留待实现）
            // https://pay.weixin.qq.com/wiki/doc/api/micropay.php?chapter=4_3
            string sign = EncryptHelper.GetHmacSha256(sb.ToString(), value).ToUpper();

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
                if (v != null && Regex.IsMatch(v, @"^[0-9.]$"))
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
            return this.HttpContext.Request.Headers["charset"];
        }
    }
}
