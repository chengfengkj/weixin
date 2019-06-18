using System;
using System.Collections.Generic;
using System.Text;
using WeiXin.Pay;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 沙箱环境下 签名key请求
    /// </summary>
    [Serializable]
    public class RequestSignKey
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 签名类型
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        public readonly RequestHandler PackageRequestHandler;
        public readonly string Sign;

        /// <summary>
        /// 关闭订单 请求参数
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="mchId"></param>
        /// <param name="outTradeNo"></param>
        /// <param name="key"></param>
        /// <param name="nonceStr"></param>
        /// <param name="signType"></param>
        public RequestSignKey(string mchId, string nonceStr, string key, string signType = "MD5")
        {
            MchId = mchId;
            NonceStr = nonceStr;
            SignType = signType;
            Key = key;

            #region 设置RequestHandler

            //创建支付应答对象
            PackageRequestHandler = new RequestHandler();
            //初始化
            PackageRequestHandler.Init();

            //设置package订单参数
            PackageRequestHandler.SetParameter("mch_id", this.MchId); //商户号
            PackageRequestHandler.SetParameter("nonce_str", this.NonceStr); //随机字符串
            Sign = PackageRequestHandler.CreateMd5Sign("key", this.Key);
            PackageRequestHandler.SetParameter("sign", Sign); //签名

            #endregion
        }
    }
}
