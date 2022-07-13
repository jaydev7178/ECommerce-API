using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Ecommerce.Models
{
    public class ReturnObject
    {
        public string response_code;
        public object obj;
   
    }
    public class Return
    {
        public static HttpResponseMessage returnHttp(string response_code, Object obj)
        {
            try
            {
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                ReturnObject ro = new ReturnObject();
                ro.response_code = response_code;
                ro.obj = obj;
                
               // string json = javaScriptSerializer.Serialize(ro);
                string json = JsonConvert.SerializeObject(ro, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                return new HttpResponseMessage()
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            catch
            {
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                ReturnObject ro = new ReturnObject();
                ro.response_code = "201";
                ro.obj = "There was an error parsing the Data. Please try again.";
               // string json = javaScriptSerializer.Serialize("Parse error");
                string json = JsonConvert.SerializeObject("Parse error", new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                return new HttpResponseMessage()
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
        }

        
    }
}