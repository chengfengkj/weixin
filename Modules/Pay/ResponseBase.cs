﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WeiXin.Extensions;

namespace WeiXin.Modules.Pay
{
    /// <summary>
    /// 基础返回结果（微信支付返回结果基类）
    /// </summary>
    [Serializable]
    public class ResponseBase
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }

        protected XDocument _resultXml;

        /// <summary>
        /// XML内容
        /// </summary>
        public string ResultXml
        {
            get
            {
                return _resultXml.ToString();

                //StringWriter sw = new StringWriter();
                //XmlTextWriter xmlTextWriter = new XmlTextWriter(sw);
                //_resultXml.WriteTo(xmlTextWriter);
                //return sw.ToString();
            }
        }

        public ResponseBase(string resultXml)
        {
            _resultXml = XDocument.Parse(resultXml);
            return_code = GetXmlValue("return_code"); // res.Element("xml").Element
            if (!IsReturnCodeSuccess())
            {
                return_msg = GetXmlValue("return_msg"); // res.Element("xml").Element
            }
        }

        /// <summary>
        /// 获取Xml结果中对应节点的值
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public string GetXmlValue(string nodeName)
        {
            if (_resultXml == null || _resultXml.Element("xml") == null
                || _resultXml.Element("xml").Element(nodeName) == null)
            {
                return "";
            }
            return _resultXml.Element("xml").Element(nodeName).Value;
        }

        /// <summary>
        /// 获取Xml结果中对应节点的集合值
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public IList<T> GetXmlValues<T>(string nodeName)
        {
            var result = new List<T>();
            try
            {
                if (_resultXml != null)
                {
                    var xElement = _resultXml.Element("xml");
                    if (xElement != null)
                    {
                        var nodeList = xElement.Elements().Where(z => z.Name.ToString().StartsWith(nodeName));

                        result = nodeList.Select(z =>
                        {
                            try
                            {
                                return (z.Value as IConvertible).ConvertTo<T>();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }).ToList();
                    }
                }
            }
            catch (System.Exception)
            {
                result = null;
            }
            return result;
        }


        public bool IsReturnCodeSuccess()
        {
            return return_code == "SUCCESS";
        }
    }
}
