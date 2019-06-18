using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using WeiXin.Modules.Pay;
using System.IO;
using WeiXin.Util;

namespace WeiXin.Pay
{
    /// <summary>
    /// 支付接口
    /// </summary>
    public class PaymentApi
    {
        static string USER_AGENT = string.Format("WXPaySDK/1.0.0.1 ({0}) .net/{1}", Environment.OSVersion, Environment.Version);
        /**
         * 调用统一下单接口请求订单
         *  接收支付通知
         *  查询支付结果
         *  订单退款-部分退款
         * */

        /// <summary>
        /// 统一支付接口，可接受JSAPI/NATIVE/APP 下预支付订单，返回预支付订单号。NATIVE 支付返回二维码code_url。
        /// </summary>
        /// <param name="dataInfo">微信支付需要post的Data数据</param>
        /// <returns></returns>
        public static ResponseUnifiedOrder Unifiedorder(RequestUnifiedOrder dataInfo)
        {
            var urlFormat = ReurnPayApiUrl("https://api.mch.weixin.qq.com/{0}pay/unifiedorder");
            var hi = new HttpItem
            {
                URL = urlFormat,
                Method = "POST",
                PostEncoding = Encoding.UTF8,
                ContentType = "text/xml",
                UserAgent= USER_AGENT,
                Postdata = dataInfo.PackageRequestHandler.ParseXML(),       //获取XML
            };
            var body = WebUtils.PayPost(dataInfo.PackageRequestHandler.ParseXML(), urlFormat, false, null, null,out string msg);
            if (!string.IsNullOrEmpty(msg))
            {
                logger.LogError($"调用微信Unifiedorder 失败，url:{urlFormat},msg:{msg}");
                return null;
            }
            return new ResponseUnifiedOrder(body);
        }

        /// <summary>
        /// 获取UI使用的JS支付签名
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonceStr"></param>
        /// <param name="package">格式：prepay_id={0}</param>
        /// <param name="signType"></param>
        /// <returns></returns>
        public static string GetJsPaySign(string appId, string timeStamp, string nonceStr, string package,
            string signType = "MD5")
        {
            //设置支付参数
            RequestHandler paySignReqHandler = new RequestHandler();
            paySignReqHandler.SetParameter("appId", appId);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("package", package);
            paySignReqHandler.SetParameter("signType", signType);
            var paySign = paySignReqHandler.CreateMd5Sign("key", Config.WeixinSetting.PayKey);
            return paySign;
        }


        /// <summary>
        /// 获取沙箱环境验签秘钥API
        /// </summary>
        /// <param name="mchId">商户号</param>
        /// <param name="nonceStr">随机字符串</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public static ResponseSignKey GetSignKey(RequestSignKey dataInfo)
        {
            var url = "https://api.mch.weixin.qq.com/sandboxnew/pay/getsignkey";

            var hi = new HttpItem
            {
                URL = url,
                Method = "POST",
                Postdata = dataInfo.PackageRequestHandler.ParseXML(),       //获取XML
            };
            var hr = WebUtils.GetHtml(hi);
            if (hr == null || !string.IsNullOrEmpty(hr.StatusDescription))
            {
                logger.LogError($"调用微信 GetSignKey 失败，url:{url},msg:{hr.StatusDescription}");
                return null;
            }
            return new ResponseSignKey(hr.Html);
        }


        static ILogger logger;
        static PaymentApi()
        {
            logger = Config.ApplicationServices.GetService<ILogger<PaymentApi>>();
        }

        /// <summary>
        /// 返回可用的微信支付地址（自动判断是否使用沙箱）
        /// </summary>
        /// <param name="urlFormat">如：<code>https://api.mch.weixin.qq.com/{0}pay/unifiedorder</code></param>
        /// <returns></returns>
        static string ReurnPayApiUrl(string urlFormat)
        {
            return string.Format(urlFormat, Config.WeixinSetting.IsDebug ? "sandboxnew/" : "");
        }

    }
}
