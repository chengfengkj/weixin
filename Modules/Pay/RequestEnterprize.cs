using System;
using System.Collections.Generic;
using System.Text;
using WeiXin.Pay;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 支付：企业付款请求实体
    /// </summary>
    [Serializable]
    public class RequestEnterprize
    {

        public RequestEnterprize()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="mchId">商户号</param>
        /// <param name="nonceStr">随机字符串</param>
        /// <param name="outTradeNo">外部订单号</param>
        /// <param name="openId"></param>
        /// <param name="amount">转账金额，单位分</param>
        /// <param name="remark">转账备注</param>
        /// <param name="ip"></param>
        /// <param name="deviceInfo"></param>
        public RequestEnterprize(string appId,string mchId,string nonceStr,string outTradeNo,string openId,int amount,string remark,string ip,string deviceInfo=null)
        {
            AppId = appId;
            MchId = mchId;
            DeviceInfo = deviceInfo;
            NonceStr = nonceStr;
            OutTradeNo = outTradeNo;
            OpenId = openId;
            TotalFee = amount;
            Remark = remark;
            Ip = ip;
            Key = Config.WeixinSetting.PayKey;

            #region 设置RequestHandler

            //创建支付应答对象
            PackageRequestHandler = new RequestHandler();
            //初始化
            PackageRequestHandler.Init();

            //设置package订单参数
            //以下设置顺序按照官方文档排序，方便维护：https://pay.weixin.qq.com/wiki/doc/api/tools/mch_pay.php?chapter=14_2
            PackageRequestHandler.SetParameter("mch_appid", this.AppId);                       //公众账号ID
            PackageRequestHandler.SetParameter("mchid", this.MchId);                      //商户号
            PackageRequestHandler.SetParameterWhenNotNull("device_info", this.DeviceInfo); //自定义参数
            PackageRequestHandler.SetParameter("nonce_str", this.NonceStr);                //随机字符串
            PackageRequestHandler.SetParameter("partner_trade_no", this.OutTradeNo);           //商家订单号
            PackageRequestHandler.SetParameterWhenNotNull("openid", this.OpenId);                     //用户的openId
            PackageRequestHandler.SetParameterWhenNotNull("check_name", "NO_CHECK");       //不校验真实姓名
            PackageRequestHandler.SetParameter("amount", this.TotalFee.ToString());     //商品金额,以分为单位(money * 100).ToString()
            PackageRequestHandler.SetParameter("spbill_create_ip", this.Ip);   //用户的公网ip，不是商户服务器IP
            PackageRequestHandler.SetParameterWhenNotNull("desc", this.Remark);   //企业付款备注，必填。注意：备注中的敏感词会被转成字符*

            Sign = PackageRequestHandler.CreateMd5Sign("key", this.Key);

            PackageRequestHandler.SetParameter("sign", Sign);                              //签名
            #endregion

        }

        public readonly RequestHandler PackageRequestHandler;
        public readonly string Sign;

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"，String(32)如：013467007045764
        /// </summary>
        public string DeviceInfo { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; }
        /// <summary>
        /// 商家订单号
        /// </summary>
        public string OutTradeNo { get; set; }
        /// <summary>
        /// 用户的openId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 商品金额,以分为单位(money * 100).ToString()
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 付款备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 用户实际ip
        /// </summary>
        public string Ip { get; set; }
    }
}
