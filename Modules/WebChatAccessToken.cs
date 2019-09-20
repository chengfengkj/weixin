using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Modules
{
    public class OAuthAccessTokenResult: AccessToken
    {
        public string refresh_token { get; set; }

        public string openid { get; set; }

        public string scope { get; set; }
    }
}
