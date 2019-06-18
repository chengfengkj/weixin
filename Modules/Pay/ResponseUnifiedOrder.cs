using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 统一下单响应
    /// </summary>
    [Serializable]
    public class ResponseUnifiedOrder : ResponseSuccess
    {
        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 交易类型:JSAPI、NATIVE、APP
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 微信生成的预支付ID，用于后续接口调用中使用
        /// </summary>
        public string prepay_id { get; set; }

        /// <summary>
        /// trade_type为NATIVE时有返回，此参数可直接生成二维码展示出来进行扫码支付
        /// </summary>
        public string code_url { get; set; }

        /// <summary>
        /// 在H5支付时返回
        /// </summary>
        public string mweb_url { get; set; }

        ///// <summary>
        ///// 子商户公众账号ID
        ///// </summary>
        //public string sub_appid { get; set; }

        ///// <summary>
        ///// 子商户号
        ///// </summary>
        //public string sub_mch_id { get; set; }

        public ResponseUnifiedOrder(string resultXml) : base(resultXml)
        {
            if (base.IsReturnCodeSuccess())
            {
                device_info = GetXmlValue("device_info") ?? "";
                //sub_appid = GetXmlValue("sub_appid") ?? "";
                //sub_mch_id = GetXmlValue("sub_mch_id") ?? "";

                if (base.IsResultCodeSuccess())
                {
                    trade_type = GetXmlValue("trade_type") ?? "";
                    prepay_id = GetXmlValue("prepay_id") ?? "";
                    code_url = GetXmlValue("code_url") ?? "";
                    mweb_url = GetXmlValue("mweb_url") ?? "";
                }
            }
        }
    }
}
