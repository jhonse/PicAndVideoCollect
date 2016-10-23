﻿using GoogleBloggerPublic.Lib.jPublic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib
{
    class JUrl
    {
        private static CookieContainer cookie = new CookieContainer();

        public static string getHtml(string name, string desc, List<string> LocalPic, string type, string xmlrpcUrl = "", string xmlrpcUsername="", string xmlrpcPassword="") {
            string html = "";
            int i = 0;
            foreach (string lp in LocalPic) {
                if (type == "Flickr")
                {
                    string photoid = jFlickr.jFlickr.uploadPic(lp, name, desc);
                    if (photoid == "")
                    {
                        continue;
                    }
                    string[] lcPic = jFlickr.jFlickr.getPicUrl(photoid);
                    if (lcPic[0].Equals(""))
                    {
                        continue;
                    }
                    html += "<div class='separator' style='clear: both; text - align: center; '>";
                    html += "<a href='" + lcPic[2] + "' imageanchor = '1' target='_blank'  title='" + name + "' >";
                    html += "<img border='0' src='" + lcPic[1] + "' ondragstart='return false;' alt='" + name + "' />";
                    html += "</a></div><br/>";
                    if (i == 3)
                    {
                        html += "<!--more-->";
                    }
                }
                else if (type == "XMLRPC") {
                    try
                    {
                        jXmlRpc xmlRpc = new jXmlRpc();
                        xmlRpc.Url = "http://girl.picturehub.net/xmlrpc.php";
                        WordPressUploadData uploadData = new WordPressUploadData();
                        uploadData.name = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + Path.GetExtension(lp);
                        uploadData.type = jFile.getMimeType(lp);
                        uploadData.bits = jFile.getBits(lp);
                        uploadData.overwrite = true;
                        WordPressUploadDataResult rs = xmlRpc.newMediaObject("blog_id", xmlrpcUsername, xmlrpcPassword, uploadData);
                        if (!rs.url.Equals(""))
                        {
                            html += "<a href='"+ rs.url + "'>";
                            html += "<img class='aligncenter size-full wp-image-" + i.ToString() + "' src='" + rs.url + "' alt='" + name + "' />";
                            html += "</a>";
                            html += "<!--nextpage-->";
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                i++;
            }
            return html;
        }

        public static string HttpPost(string Url, string postDataStr)
        {
            string retString = "";
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postDataStr); //转化

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(byteArray, 0, byteArray.Length);
                myRequestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                response.Cookies = cookie.GetCookies(response.ResponseUri);
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                retString = ex.Message;
            }

            return retString;
        }

        public static string HttpGet(string Url, string postDataStr)
        {
            string retString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception)
            {

            }
            return retString;
        }
    }
}
