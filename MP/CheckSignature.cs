using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WeiXin.Util;

namespace WeiXin.MP
{
    /// <summary>
    /// 微信服务器验证
    /// </summary>
    public class CheckSignature
    {
        /// <summary>
        /// 检查签名是否正确
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool Check(string signature, string timestamp, string nonce)
        {
            return signature == GetSignature(timestamp, nonce);
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce)
        {
            var arr = new[] { Config.WeixinSetting.Token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            //var enText = FormsAuthentication.HashPasswordForStoringInConfigFile(arrString, "SHA1");//使用System.Web.Security程序集
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="jsapiTicket"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce,string jsapiTicket,string url)
        {
            var dic = new Dictionary<string, string>
            {
                {"noncestr",nonce },
                {"timestamp",timestamp },
                {"jsapi_ticket",jsapiTicket },
                {"url",url },
            };
            var arrString = string.Join("&", dic.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}"));
            //var enText = FormsAuthentication.HashPasswordForStoringInConfigFile(arrString, "SHA1");//使用System.Web.Security程序集
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }

        public static bool Check(string signature, string timestamp, string nonce, string Token)
        {
            var sign = "";
            WXBizMsgCrypt.GenarateSinature(Token, timestamp, nonce, Config.WeixinSetting.EncodingAESKey, ref sign);
            return sign.Equals(signature);
        }
    }
}
