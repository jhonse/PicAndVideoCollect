using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib
{
    class jDownLoad
    {
        private static CookieContainer cookie = new CookieContainer();
        private static string contentType = "application/x-www-form-urlencoded";
        private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";

        public static string getUrl(int current_num,string url) {
            if (url.Contains("***")) {
                string[] url_arr = url.Split(new string[] { "***" }, StringSplitOptions.RemoveEmptyEntries);
                if (url_arr.Length == 2)
                {
                    string filePath = url_arr[0];
                    string fileName = current_num.ToString() + url_arr[1];
                    return filePath + fileName;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public static List<string[]> getLists(string url,string type="mm131")
        {
            List<string[]> ret = new List<string[]>();
            if (!url.Equals(""))
            {
                string ListPageData = downPage(url);
                if (type == "mm131")
                {
                    return Analysismm131(ListPageData);
                }
                else if (type == "8090mg")
                {
                    return Analysis8090mg(ListPageData);
                }
                else if (type == "gogorenti") {
                    return Analysisgogorenti(ListPageData);
                }
                else if(type == "yesky")
                {
                    return AnalysisYesky(ListPageData);
                }
                else if (type == "tesetu")
                {
                    return AnalysisTesetu(ListPageData);
                }
                else
                {
                    return Analysismm131(ListPageData);
                }
            }
            return ret;
        }

        public static List<string> getImageUrl(int page, string imageUrl, string parentName, string imagePath, string type = "mm131", string jPicPositionStartX = "", string jPicPositionStartY = "", string jPicPositionEndX = "", string jPicPositionEndY = "", string jPicWaterText = "", string jPicWaterPosition = "", bool jPicCutOpen = false, bool jPicWaterOpen = false, bool PicBackup = false) {
            List<string> ret = new List<string>();
            if (!imageUrl.Equals("") && !parentName.Equals("") && !imagePath.Equals(""))
            {
                if(jFile.is_Dir(imagePath + "/" + page.ToString() + "/" + parentName))
                {
                    if (type == "mm131")
                    {
                        ret = getImageUrlmm131(page, imageUrl, parentName, imagePath);
                    }
                    else if (type == "8090mg")
                    {
                        ret = getImage8090mg(page, imageUrl, parentName, imagePath);
                    }
                    else if (type == "gogorenti")
                    {
                        ret = getImagegogorenti(page, imageUrl, parentName, imagePath);
                    }
                    else if(type == "yesky")
                    {
                        ret = getImageYes(page, imageUrl, parentName, imagePath);
                    }
                    else if (type == "tesetu")
                    {
                        ret = getImageTesetu(page, imageUrl, parentName, imagePath);
                    }
                    else
                    {
                        ret = getImageUrlmm131(page, imageUrl, parentName, imagePath);
                    }
                }
            }
            for (int i = 0, count = ret.Count; i < count; i++)
            {
                if (jPicCutOpen)
                {
                    string extName = Path.GetExtension(ret[i]);
                    string pathName = ret[i].Substring(0, ret[i].Length - extName.Length);
                    if (jImage.CutImage(ret[i], pathName + "_c" + extName, int.Parse(jPicPositionStartX), int.Parse(jPicPositionStartY), int.Parse(jPicPositionEndX), int.Parse(jPicPositionEndY), extName.Substring(1)))
                    {
                        if(!PicBackup)
                            jFile.deleteFile(ret[i]);
                        ret[i] = pathName + "_c" + extName;
                    }
                }
                if (jPicWaterOpen)
                {
                    string extName = Path.GetExtension(ret[i]);
                    string pathName = ret[i].Substring(0, ret[i].Length - extName.Length);
                    if (jImage.addWaterMark(ret[i], pathName + "_w" + extName, jPicWaterText, "", jPicWaterPosition))
                    {
                        if(!PicBackup)
                            jFile.deleteFile(ret[i]);
                        ret[i] = pathName + "_w" + extName;
                    }
                }
            }
            return ret;
        }

        private static List<string[]> Analysismm131(string Data) {
            List<string[]> ret = new List<string[]>();
            try
            {
                if (!Data.Equals(""))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(Data);
                    HtmlNode dts = doc.DocumentNode.SelectSingleNode("//dl[@class='list-left public-box']");
                    if (dts != null)
                    {
                        HtmlNodeCollection dds = HtmlNode.CreateNode(dts.OuterHtml).SelectNodes("//dd");
                        dds.Remove(dds.Count - 1);
                        if (dds.Count > 0)
                        {
                            foreach (HtmlNode dd in dds)
                            {
                                string[] data = new string[2];
                                HtmlNode img = HtmlNode.CreateNode(dd.OuterHtml).SelectSingleNode("//img");
                                string src = img.Attributes["src"].Value;
                                string title = img.Attributes["alt"].Value;
                                if (!src.Equals("") && !title.Equals(""))
                                {
                                    data[0] = title;
                                    data[1] = src;
                                    ret.Add(data);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { 
            
            }
            return ret;
        }

        private static List<string[]> Analysis8090mg(string Data) {
            List<string[]> ret = new List<string[]>();
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(Data);
                HtmlNode uls = doc.DocumentNode.SelectSingleNode("//ul[@id='piclist']");
                if (uls != null) {
                    HtmlNodeCollection lis = HtmlNode.CreateNode(uls.OuterHtml).SelectNodes("//li[@class='piclist_li']");
                    if (lis.Count > 0) {
                        foreach (HtmlNode li in lis) {
                            string[] data = new string[2];
                            HtmlNode n_title = HtmlNode.CreateNode(li.OuterHtml).SelectSingleNode("//a[@class='piclist_a']");
                            string src = "http://www.8090mg.com/" + n_title.Attributes["href"].Value;
                            string title = n_title.InnerHtml;
                            if (!src.Equals("") && !title.Equals(""))
                            {
                                data[0] = title;
                                data[1] = src;
                                ret.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception) {

            }
            return ret;
        }

        private static List<string[]> Analysisgogorenti(string Data)
        {
            List<string[]> ret = new List<string[]>();
            try
            {
                if (!Data.Equals(""))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(Data);
                    HtmlNode dts = doc.DocumentNode.SelectSingleNode("//ul[@class='ulPic']");
                    if (dts != null)
                    {
                        HtmlNodeCollection dds = HtmlNode.CreateNode(dts.OuterHtml).SelectNodes("//li");
                        dds.Remove(dds.Count - 1);
                        if (dds.Count > 0)
                        {
                            foreach (HtmlNode dd in dds)
                            {
                                string[] data = new string[2];
                                HtmlNode img = HtmlNode.CreateNode(dd.OuterHtml).SelectSingleNode("//img");
                                string src = img.Attributes["src"].Value;
                                string title = img.Attributes["alt"].Value;
                                if (!src.Equals("") && !title.Equals(""))
                                {
                                    data[0] = title;
                                    data[1] = src;
                                    ret.Add(data);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return ret;
        }

        private static List<string[]> AnalysisYesky(string Data)
        {
            List<string[]> ret = new List<string[]>();
            try
            {
                if (!Data.Equals(""))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(Data);
                    HtmlNode div = doc.DocumentNode.SelectSingleNode("//div[@class='lb_box']");
                    if (div != null)
                    {
                        HtmlNodeCollection dls = HtmlNode.CreateNode(div.OuterHtml).SelectNodes("//dl");
                        if(dls.Count > 0)
                        {
                            foreach(HtmlNode dl in dls)
                            {
                                HtmlNode dt = HtmlNode.CreateNode(dl.OuterHtml).SelectSingleNode("//dt");
                                if(dt != null)
                                {
                                    HtmlNode a = HtmlNode.CreateNode(dt.OuterHtml).SelectSingleNode("//a");
                                    if(a != null)
                                    {
                                        HtmlNode img = HtmlNode.CreateNode(a.OuterHtml).SelectSingleNode("//img");
                                        if(img != null)
                                        {
                                            string[] data = new string[2];
                                            string src = a.Attributes["href"].Value;
                                            string title = img.Attributes["alt"].Value;
                                            if (!src.Equals("") && !title.Equals(""))
                                            {
                                                data[0] = title;
                                                data[1] = src;
                                                ret.Add(data);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return ret;
        }

        private static List<string[]> AnalysisTesetu(string Data)
        {
            List<string[]> ret = new List<string[]>();
            try
            {
                if (!Data.Equals(""))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(Data);
                    HtmlNode tag_ssxz = doc.DocumentNode.SelectSingleNode("//div[@class='tag_ssxz']");
                    if (tag_ssxz != null)
                    {
                        HtmlNode ul = HtmlNode.CreateNode(tag_ssxz.OuterHtml).SelectSingleNode("//ul");
                        if(ul != null)
                        {
                            HtmlNodeCollection lis = HtmlNode.CreateNode(ul.OuterHtml).SelectNodes("//li");
                            if (lis.Count > 0)
                            {
                                foreach (HtmlNode li in lis)
                                {
                                    HtmlNode a = HtmlNode.CreateNode(li.OuterHtml).SelectSingleNode("//a");
                                    if(a != null)
                                    {
                                        HtmlNode img = HtmlNode.CreateNode(a.OuterHtml).SelectSingleNode("//img");
                                        HtmlNode h4 = HtmlNode.CreateNode(a.OuterHtml).SelectSingleNode("//h4");
                                        if(img != null && h4 != null)
                                        {
                                            string[] data = new string[2];
                                            string src = img.Attributes["src"].Value;
                                            string title = h4.InnerText;
                                            if (!src.Equals("") && !title.Equals(""))
                                            {
                                                data[0] = title;
                                                data[1] = src;
                                                ret.Add(data);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return ret;
        }

        private static List<string> getImageUrlmm131(int page, string imageUrl, string parentName, string imagePath) {
            List<string> ret = new List<string>();
            string extName = Path.GetExtension(imageUrl);
            string picfilename = Path.GetFileName(imageUrl);
            string pathName = imageUrl.Substring(0, imageUrl.Length - picfilename.Length);
            for (int i = 1; ; i++)
            {
                try
                {
                    string fileName = i.ToString() + extName;
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(pathName + fileName, imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName);
                    //if(ret.Count < 2)
                    if(jImage.is_pic(imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName))
                    {
                        ret.Add(imagePath + "\\" + page.ToString() + "\\" + parentName + "\\" + fileName);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            return ret;
        }

        private static List<string> getImage8090mg(int page, string imageUrl, string parentName, string imagePath) {
            List<string> ret = new List<string>();
            string extName = Path.GetExtension(imageUrl);
            string pathName = imageUrl.Substring(0, imageUrl.Length - extName.Length);
            for (int i = 1; ; i++) {
                try
                {
                    string imgPageUrl = "";
                    if (i == 1) {
                        imgPageUrl = pathName + extName;
                    }else
                    {
                        imgPageUrl = pathName + "_" + i.ToString() + extName;
                    }
                    string pageData = downPage(imgPageUrl);
                    if (!pageData.Equals("")) {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(pageData);
                        HtmlNode content = doc.DocumentNode.SelectSingleNode("//div[@id='contents']");
                        if (content != null) {
                            HtmlNode img = HtmlNode.CreateNode(content.OuterHtml).SelectSingleNode("//img");
                            if (img == null) {
                                HtmlNodeCollection ps = HtmlNode.CreateNode(content.OuterHtml).SelectNodes("//p");
                                foreach (HtmlNode p in ps) {
                                    img = HtmlNode.CreateNode(p.OuterHtml).SelectSingleNode("//img");
                                    if (img != null) {
                                        break;
                                    }
                                }
                            }
                            if (img != null) {
                                string picUrl = img.Attributes["src"].Value;
                                if (!picUrl.Equals("")) {
                                    if (!picUrl.Contains("www.8090mg.com")) {
                                        picUrl = "http://www.8090mg.com" + picUrl;
                                    }  
                                    string picExtName = Path.GetExtension(picUrl);
                                    WebClient mywebclient = new WebClient();
                                    mywebclient.DownloadFile(picUrl, imagePath + "/" + page.ToString() + "/" + parentName + "/" + i.ToString() + picExtName);
                                    //if(ret.Count < 2)
                                    if(jImage.is_pic(imagePath + "/" + page.ToString() + "/" + parentName + "/" + i.ToString() + picExtName))
                                    {
                                        ret.Add(imagePath + "/" + page.ToString() + "/" + parentName + "/" + i.ToString() + picExtName);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }else
                    {
                        break;
                    }
                }
                catch (Exception) {
                    break;
                }
            }
            return ret;
        }

        private static List<string> getImagegogorenti(int page, string imageUrl, string parentName, string imagePath) {
            List<string> ret = new List<string>();
            string extName = Path.GetExtension(imageUrl);
            string picfilename = Path.GetFileName(imageUrl);
            string pathName = imageUrl.Substring(0, imageUrl.Length - picfilename.Length);
            for (int i = 1; ; i++)
            {
                try
                {
                    string fileName = (i <10 ? "0"+i.ToString() : i.ToString()) + extName;
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(pathName + fileName, imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName);
                    //if(ret.Count < 2)
                    if(jImage.is_pic(imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName))
                    {
                        ret.Add(imagePath + "\\" + page.ToString() + "\\" + parentName + "\\" + fileName);
                    }
                    else
                    {
                        string fileName_o = i.ToString() + extName;
                        WebClient mywebclient_o = new WebClient();
                        mywebclient_o.DownloadFile(pathName + fileName_o, imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName_o);
                        if (jImage.is_pic(imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName_o))
                        {
                            ret.Add(imagePath + "\\" + page.ToString() + "\\" + parentName + "\\" + fileName_o);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            return ret;
        }

        private static List<string> getImageYes(int page, string imageUrl, string parentName, string imagePath)
        {
            List<string> ret = new List<string>();
            string extName = Path.GetExtension(imageUrl);
            string pathName = imageUrl.Substring(0, imageUrl.Length - extName.Length);
            for (int i = 1; ; i++)
            {
                try
                {
                    string imgPageUrl = "";
                    if (i == 1)
                    {
                        imgPageUrl = pathName + extName;
                    }
                    else
                    {
                        imgPageUrl = pathName + "_" + i.ToString() + extName;
                    }
                    string pageData = downPage(imgPageUrl);
                    if (!pageData.Equals(""))
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(pageData);
                        HtmlNode l_effect_img = doc.DocumentNode.SelectSingleNode("//div[@id='l_effect_img']");
                        if (l_effect_img != null)
                        {
                            HtmlNode l_effect_img_mid = HtmlNode.CreateNode(l_effect_img.OuterHtml).SelectSingleNode("//div[@class='l_effect_img_mid']");
                            if(l_effect_img_mid != null)
                            {
                                HtmlNode a = HtmlNode.CreateNode(l_effect_img_mid.OuterHtml).SelectSingleNode("//a");
                                if(a != null)
                                {
                                    HtmlNode img = HtmlNode.CreateNode(a.OuterHtml).SelectSingleNode("//img");
                                    if(img != null)
                                    {
                                        string picUrl = img.Attributes["src"].Value;
                                        if (!picUrl.Equals(""))
                                        {
                                            string picExtName = Path.GetExtension(picUrl);
                                            WebClient mywebclient = new WebClient();
                                            mywebclient.DownloadFile(picUrl, imagePath + "/" + page.ToString() + "/" + parentName + "/" + i.ToString() + picExtName);
                                            //if(ret.Count < 2)
                                            if (jImage.is_pic(imagePath + "/" + page.ToString() + "/" + parentName + "/" + i.ToString() + picExtName))
                                            {
                                                ret.Add(imagePath + "/" + page.ToString() + "/" + parentName + "/" + i.ToString() + picExtName);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            return ret;
        }

        private static List<string> getImageTesetu(int page, string imageUrl, string parentName, string imagePath)
        {
            List<string> ret = new List<string>();
            string extName = Path.GetExtension(imageUrl);
            string picfilename = Path.GetFileName(imageUrl);
            string pathName = imageUrl.Substring(0, imageUrl.Length - picfilename.Length);
            for (int i = 1; ; i++)
            {
                try
                {
                    string fileName = (i < 10 ? "0" + i.ToString() : i.ToString()) + extName;
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(pathName + fileName, imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName);
                    //if(ret.Count < 2)
                    if (jImage.is_pic(imagePath + "/" + page.ToString() + "/" + parentName + "/" + fileName))
                    {
                        ret.Add(imagePath + "\\" + page.ToString() + "\\" + parentName + "\\" + fileName);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            return ret;
        }

        private static string downPage(string url) {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = userAgent;
                request.ContentType = contentType;
                request.CookieContainer = cookie;
                request.Accept = accept;
                request.Method = "get";

                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.Default);
                String html = reader.ReadToEnd();
                response.Close();

                return html;
            }
            catch (Exception) {
                return "";
            }
        }
    }
}
