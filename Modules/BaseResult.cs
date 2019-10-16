using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Modules
{
    public  class BaseResult
    {
        /// <summary>
        /// 错误码，，错误时存在
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误消息，，错误时存在
        /// </summary>
        public string errmsg { get; set; }
    }
}
