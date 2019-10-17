using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WeiXin.Enums;
using WeiXin.Extensions;
using WeiXin.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace WeiXin.MP
{
    public  class OAuth
    {
        static ILogger logger;
        static OAuth()
        {
            logger=Config.ApplicationServices.GetService<ILogger<OAuth>>();
        }

        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static string GetAuthorizeUrl(string appId, string redirectUrl, string state, OAuthScope scope, string responseType = "code")
        {
            var url =
                string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect",
                                appId, redirectUrl.UrlEncode(), responseType, scope, state);

            /* 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
             * 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。这里的code用于换取access_token（和通用接口的access_token不通用）
             * 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
             */
            return url;
        }

        /// <summary>
        /// 获取client_credential的token
        /// </summary>
        /// <returns></returns>
        public static AccessToken GetAccessToken()
        {
            var url = $"{Config.WeixinSetting.ApiDomain}/cgi-bin/token?appid={Config.WeixinSetting.AppId}&secret={Config.WeixinSetting.AppSecret}&grant_type=client_credential";

            using (var client = new System.Net.Http.HttpClient())
            {
                var getresult = client.GetAsync(url);
                var result = getresult.Result.Content.ReadAsStringAsync();
                var rs = result.Result;
                if (!string.IsNullOrWhiteSpace(rs) && rs.Contains("errcode"))
                {
                    //异常
                    logger.LogError($"获取AccessToken异常，错误码{rs}");
                }
                return JsonConvert.DeserializeObject<AccessToken>(rs);
            }
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public static OAuthAccessTokenResult GetAccessToken(string code, string grantType = "authorization_code")
        {
            return GetAccessToken(Config.WeixinSetting.AppId, Config.WeixinSetting.AppSecret, code, grantType);
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public static OAuthAccessTokenResult GetAccessToken(string appId, string secret, string code, string grantType = "authorization_code")
        {
            var url = $"{Config.WeixinSetting.ApiDomain}/sns/oauth2/access_token?appid={appId}&secret={secret}&code={code}&grant_type={grantType}";

            using (var client = new System.Net.Http.HttpClient())
            {
                var getresult = client.GetAsync(url);
                var result = getresult.Result.Content.ReadAsStringAsync();
                var rs = result.Result;
                if (!string.IsNullOrWhiteSpace(rs) && rs.Contains("errcode"))
                {
                    //异常
                    logger.LogError($"获取AccessToken异常，错误码{rs}");
                }
                return JsonConvert.DeserializeObject<OAuthAccessTokenResult>(rs);
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(OAuthAccessTokenResult accessToken)
        {
            var url = string.Concat("https://api.weixin.qq.com/sns/userinfo?access_token=", 
                accessToken.access_token, "&openid=", accessToken.openid, "&lang=zh_CN");
            using (var clinet = new System.Net.Http.HttpClient())
            {
                var result = clinet.GetAsync(url)
                    .Result
                    .Content
                    .ReadAsStringAsync();
                //_logger.LogInformation(string.Concat("获取用户信息返回=", result.Result));
                if (!string.IsNullOrWhiteSpace(result.Result))
                {
                    //_logger.LogInformation("用户信息:{0}", result.Result);
                    return JsonConvert.DeserializeObject<UserInfo>(result.Result);
                }
                else
                {
                    //_logger.LogInformation("获取用户信息失败:{0}", result.Result);
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取JSApi用的ticket
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JSApiTicket GetTicket(string token)
        {
            var url = $"{Config.WeixinSetting.ApiDomain}/cgi-bin/ticket/getticket?access_token={token}&type=jsapi";

            using (var client = new System.Net.Http.HttpClient())
            {
                var getresult = client.GetAsync(url);
                var result = getresult.Result.Content.ReadAsStringAsync();
                var rs = result.Result;
                return JsonConvert.DeserializeObject<JSApiTicket>(rs);
            }
        }
    }
}
