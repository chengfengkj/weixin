using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeixinServices(this IServiceCollection services, IConfiguration configuration)
        {
            var weixinSetting = new WeixinSetting();
            configuration.Bind("WeixinSetting", weixinSetting);
            services.AddSingleton(Options.Create(weixinSetting));
            Config.WeixinSetting = weixinSetting;
            return services;
        }
    }
}
