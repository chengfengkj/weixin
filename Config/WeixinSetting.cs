﻿namespace WeiXin
{
    public class WeixinSetting
    {
        public string Token { get; set; }

        public string EncodingAESKey { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string ApiDomain { get;set; }

        /// <summary>
        /// 微信支付appID,
        /// </summary>
        public string PayAppID { get; set; }

        /// <summary>
        /// 微信支付密钥：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
        /// </summary>
        public string PayKey { get; set; }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string PayMchID { get; set; }

        /// <summary>
        /// 证书路径，，win下需要先双击安装证书
        /// </summary>
        public string CertPath { get; set; }

        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertPwd { get; set; }
    }
}
