using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin
{
    public class Config
    {
        /// <summary>
        /// ioc 容器
        /// </summary>
        public static IServiceProvider ApplicationServices { get; set; } = null;

        /// <summary>
        /// 微信api域名，默认 https://api.weixin.qq.com
        /// </summary>
        public static string ApiDomain { get; set; } = "https://api.weixin.qq.com";
    }
}
