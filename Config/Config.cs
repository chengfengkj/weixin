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
        public static WeixinSetting WeixinSetting { get; set; }
    }
}
