using System;
using System.Collections.Generic;
using System.Text;
using WeiXin.Pay;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 支付：退款请求实体
    /// </summary>
    [Serializable]
    public class RequestRefund
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchId { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; }
        /// <summary>
        /// 签名类型，默认为MD5，支持HMAC-SHA256和MD5。（使用默认）
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户退款单号,商户系统内部的退款单号，商户系统内部唯一
        /// </summary>
        public string OutRefundNo { get; set; }

        /// <summary>
        /// 商品金额,以分为单位(money * 100).ToString()
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 退款金额,以分为单位(money * 100).ToString()
        /// </summary>
        public int RefundFee { get; set; }

        /// <summary>
        /// refund_desc 退款原因,String(80)
        /// </summary>
        public string RefundDesc { get; set; }

        public readonly RequestHandler PackageRequestHandler;
        public readonly string Sign;
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }


        public RequestRefund()
        {

        }

        public RequestRefund(string payAppId, string mchId,string payKey, string nonceStr, string outTradeNo, string outRefundNo
            , int totalFee, int refundFee,string refundDesc)
        {
            AppId = payAppId;
            MchId = mchId;
            Key = payKey;
            NonceStr = nonceStr;
            OutTradeNo = outTradeNo;
            OutRefundNo = outRefundNo;
            TotalFee = totalFee;
            RefundFee = refundFee;
            RefundDesc = refundDesc;
            SignType = "MD5";

            #region 设置RequestHandler

            //创建支付应答对象
            PackageRequestHandler = new RequestHandler();
            //初始化
            PackageRequestHandler.Init();

            //设置package订单参数
            //以下设置顺序按照官方文档排序，方便维护：https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_4
            PackageRequestHandler.SetParameter("appid", this.AppId);                       //公众账号ID
            PackageRequestHandler.SetParameter("mch_id", this.MchId);                      //商户号
            PackageRequestHandler.SetParameter("nonce_str", this.NonceStr);                //随机字符串
            PackageRequestHandler.SetParameterWhenNotNull("sign_type", this.SignType);     //签名类型，默认为MD5
            PackageRequestHandler.SetParameter("out_trade_no", this.OutTradeNo);           //商家订单号
            PackageRequestHandler.SetParameter("out_refund_no", this.OutRefundNo);           //商户退款单号
            PackageRequestHandler.SetParameter("total_fee", this.TotalFee.ToString());     //商品金额,以分为单位(money * 100).ToString()
            PackageRequestHandler.SetParameter("refund_fee", this.RefundFee.ToString());     //退款金额,以分为单位(money * 100).ToString()
            PackageRequestHandler.SetParameter("refund_desc", this.RefundDesc);   //退款原因

            Sign = PackageRequestHandler.CreateMd5Sign("key", this.Key);

            PackageRequestHandler.SetParameter("sign", Sign);                              //签名
            #endregion
        }
    }
}
