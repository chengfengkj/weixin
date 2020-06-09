using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 企业支付响应
    /// </summary>
    [Serializable]
    public class ResponseRefund : ResponseBase
    {
        public ResponseRefund(string resultXml) : base(resultXml)
        {
            result_code = GetXmlValue("result_code"); // res.Element("xml").Element

            if (base.IsReturnCodeSuccess())
            {
                appid = GetXmlValue("appid") ?? "";
                mch_id = GetXmlValue("mch_id") ?? "";
                nonce_str = GetXmlValue("nonce_str") ?? "";
                sign = GetXmlValue("sign") ?? "";
                err_code = GetXmlValue("err_code") ?? "";
                err_code_des = GetXmlValue("err_code_des") ?? "";

                if (IsResultCodeSuccess())
                {
                    out_trade_no = GetXmlValue("out_trade_no");
                    out_refund_no = GetXmlValue("out_refund_no");
                    refund_id = GetXmlValue("refund_id");
                    refund_fee = int.Parse(GetXmlValue("refund_fee"));
                }
            }
        }

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
        public string out_trade_no { get; set; }

        /// <summary>
        /// 商户退款单号,，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。
        /// </summary>
        public string out_refund_no { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public int refund_fee { get; set; }

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
