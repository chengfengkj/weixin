using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXin.Extensions
{
    public static class StringExtensions
    {
        public static string UrlEncode(this string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);
        }
    }
}
