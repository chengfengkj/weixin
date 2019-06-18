using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Enums
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 公众号JS-API支付和小程序支付
        /// </summary>
        JSAPI,
        NATIVE,
        APP,
        MWEB
    }
}
