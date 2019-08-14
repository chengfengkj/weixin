using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 企业支付响应
    /// </summary>
    [Serializable]
    public class ResponseEnterprize : ResponseBase
    {
        public ResponseEnterprize(string resultXml) : base(resultXml)
        {
            result_code = GetXmlValue("result_code"); // res.Element("xml").Element

            if (base.IsReturnCodeSuccess())
            {
                appid = GetXmlValue("mch_appid") ?? "";
                mch_id = GetXmlValue("mchid") ?? "";
                device_info = GetXmlValue("device_info") ?? "";
                nonce_str = GetXmlValue("nonce_str") ?? "";
                sign = GetXmlValue("sign") ?? "";
                err_code = GetXmlValue("err_code") ?? "";
                err_code_des = GetXmlValue("err_code_des") ?? "";

                if (IsResultCodeSuccess())
                {
                    partner_trade_no = GetXmlValue("partner_trade_no");
                    payment_no = GetXmlValue("payment_no");
                    payment_time = Convert.ToDateTime(GetXmlValue("payment_time"));
                }
            }
        }


        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 微信分配的公众账号ID（付款到银行卡接口，此字段不提供）
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 随机字符串，不长于32 位
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }

        public string err_code { get; set; }
        public string err_code_des { get; set; }

        /// <summary>
        /// 商户订单号，需保持历史全局唯一性(只能是字母或者数字，不能包含有其他字符)
        /// </summary>
        public string partner_trade_no { get; set; }

        /// <summary>
        /// 企业付款成功，返回的微信付款单号
        /// </summary>
        public string payment_no { get; set; }

        /// <summary>
        /// 企业付款成功时间
        /// </summary>
        public DateTime payment_time { get; set; }

        /// <summary>
        /// result_code == "SUCCESS"
        /// </summary>
        /// <returns></returns>
        public bool IsResultCodeSuccess()
        {
            return result_code == "SUCCESS";
        }
    }
}
