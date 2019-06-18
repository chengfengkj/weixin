using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 沙箱环境 signkey 响应
    /// </summary>
    [Serializable]
    public class ResponseSignKey: ResponseSuccess
    {
        /// <summary>
        /// 返回的沙箱密钥
        /// </summary>
        public string sandbox_signkey { get; set; }

        /// <summary>
        /// 获取验签秘钥API 返回结果 构造函数
        /// </summary>
        /// <param name="resultXml"></param>
        public ResponseSignKey(string resultXml) : base(resultXml)
        {
            if (base.IsReturnCodeSuccess())
            {
                mch_id = GetXmlValue("mch_id") ?? "";
                sandbox_signkey = GetXmlValue("sandbox_signkey") ?? "";
            }
        }
    }
}
