using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Modules
{
   public  class AccessToken: BaseResult
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

    }
}
