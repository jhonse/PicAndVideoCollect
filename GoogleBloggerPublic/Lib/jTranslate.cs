using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib
{
    class jTranslate
    {
        public static string forBaidu(string value, string appid = "20160925000029248", string key = "S9kJtRbFMRQBN2hW0ceD", string from = "zh", string to = "en") {
            string ret = "";
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    string url = "http://api.fanyi.baidu.com/api/trans/vip/translate";
                    string q = value;
                    string salt = (new Random()).Next(0, 100).ToString();
                    string sign = jString.StrToMD5(appid + q + salt + key);
                    url += "?q="+value+"&from="+from+"&to="+to+"&appid="+appid+"&salt="+salt+"&sign="+sign;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    System.IO.Stream stream = response.GetResponseStream();
                    System.IO.StreamReader streamReader = new System.IO.StreamReader(stream);

                    string result = streamReader.ReadToEnd();

                    if (!string.IsNullOrEmpty(result))
                    {
                        JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                        if(jo.Count == 3)
                        {
                            ret = jo["trans_result"][0]["dst"].ToString();
                        }
                    }

                    streamReader.Close();
                    stream.Close();
                    response.Close();
                }
                catch (Exception)
                {

                }
            }
            return ret;
        }
    }

    public class baiduResult {
        public string from { get; set; }
        public string to { get; set; }
        public object[][] trans_result { get; set; }
    }
}
