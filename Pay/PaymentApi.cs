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
        /**
         * 调用统一下单接口请求订单
         *  接收支付通知
         *  查询支付结果
         *  订单退款-部分退款
         * */

        static string USER_AGENT = string.Format("WXPaySDK/1.0.0.1 ({0}) .net/{1}", Environment.OSVersion, Environment.Version);

        /// <summary>
        /// 企业支付，付款给用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResponseEnterprize Enterprize(RequestEnterprize request, string certPath, string certPwd)
        {
            var urlFormat = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
            var body = WebUtils.PayPost(request.PackageRequestHandler.ParseXML(), urlFormat, true, certPwd, certPath, out string msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return new ResponseEnterprize($"<xml><return_code><![CDATA[SUCCESS]]></return_code><err_code_des>{msg}</err_code_des></xml>");
            }
            return new ResponseEnterprize(body);
        }

        /// <summary>
        /// 用户退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ResponseRefund Refund(RequestRefund request, string certPath, string certPwd)
        {
            var urlFormat = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            var body = WebUtils.PayPost(request.PackageRequestHandler.ParseXML(), urlFormat, true, certPwd, certPath, out string msg);
            if (!string.IsNullOrEmpty(msg))
            {
                //logger.LogError($"调用微信Refund 失败，url:{urlFormat},msg:{msg}");
                return new ResponseRefund($"<xml><return_code><![CDATA[SUCCESS]]></return_code><err_code_des>{msg}</err_code_des></xml>");
            }
            return new ResponseRefund(body);
        }

        /// <summary>
        /// 统一支付接口，可接受JSAPI/NATIVE/APP 下预支付订单，返回预支付订单号。NATIVE 支付返回二维码code_url。
        /// </summary>
        /// <param name="dataInfo">微信支付需要post的Data数据</param>
        /// <returns></returns>
        public static ResponseUnifiedOrder Unifiedorder(RequestUnifiedOrder dataInfo)
        {
            var urlFormat = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            var body = WebUtils.PayPost(dataInfo.PackageRequestHandler.ParseXML(), urlFormat, false, null, null, out string msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return new ResponseUnifiedOrder($"<xml><return_code><![CDATA[SUCCESS]]></return_code><err_code_des>{msg}</err_code_des></xml>");
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
        public static string GetJsPaySign(string appId, string payKey, string timeStamp, string nonceStr, string package,
            string signType = "MD5")
        {
            //设置支付参数
            RequestHandler paySignReqHandler = new RequestHandler();
            paySignReqHandler.SetParameter("appId", appId);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("package", package);
            paySignReqHandler.SetParameter("signType", signType);
            var paySign = paySignReqHandler.CreateMd5Sign("key", payKey);
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
            if (Config.ApplicationServices != null)
                logger = Config.ApplicationServices.GetService<ILogger<PaymentApi>>();
        }
    }
}
