using GoogleBloggerPublic.Lib.jPublic;
using PicAndVideoCollect.Lib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GoogleBloggerPublic.Lib
{
    class JThread
    {
        private string senderServerIp = "";
        private string toMailAddress = "";
        private string fromMailAddress = "";
        private string mailUsername = "";
        private string mailPassword = "";
        private string mailPort = "";
        private string PageList = "";
        private string PageNum = "";
        private string PublicTimes = "";
        private string PicPath = "";
        private string ApiKey = "";
        private string gropNum = "";
        private string PublicNextTime = "";
        private string SharedSecret = "";
        private string PublicNum = "";
        private string type = "";
        private string appid = "";
        private string key = "";
        private string from = "";
        private string to = "";
        private bool open = false;
        private string PicPositionStartX = "";
        private string PicPositionStartY = "";
        private string PicPositionEndX = "";
        private string PicPositionEndY = "";
        private string PicWaterText = "";
        private string PicWaterPosition = "";
        private bool PicBackup = false;
        private bool PicCutOpen = false;
        private bool PicWaterOpen = false;
        private bool TimerOpen = false;
        private DateTime TimerStart = new DateTime();
        private DateTime TimerStop = new DateTime();
        private bool TimerLog = false;
        private string PublicPassword = "";
        private string MailSendType = "";
        private string MailSendUrl = "";
        private string PublicType = "";
        private string XmlRpcUrl = "";
        private string XmlRpcUsername = "";
        private string XmlRpcPassword = "";
        private string PublicPicType = "";
        private string XmlPRCCat = "";

        private int page_current_index = 1;
        private int collect_current_index = 0;
        private int tag = 0;
        private int tag_send_index = 0;
        private int tag_next_index = 0;
        private int public_num = 0;
        private int tag_public_index = 100;

        private string public_title = "";
        private string public_desction = "";

        private bool tag_collect = true;
        private bool tag_send = false;
        private bool tag_next = false;
        private bool tag_public = false;

        private ListBox lbLogList = null;
        private Button btnverity = null;
        private Button btncompant = null;
        private Button btnStart = null;
        private Button btnStop = null;

        System.Timers.Timer t = null;
        System.Timers.Timer t_check = null;

        public JThread(ListBox jlbLogList, Button jbtnverity, Button jbtncompant, Button jbtnStart, Button jbtnStop)
        {
            lbLogList = jlbLogList;
            btnverity = jbtnverity;
            btncompant = jbtncompant;
            btnStart = jbtnStart;
            btnStop = jbtnStop;
            t = new System.Timers.Timer(1000);
            t.Elapsed += T_Elapsed;
            t.AutoReset = true;
            t.Enabled = false;

            t_check = new System.Timers.Timer(1000);
            t_check.Elapsed += T_check_Elapsed;
            t_check.AutoReset = true;
            t_check.Enabled = false;
        }

        private void T_check_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if(DateTime.Now.Hour == TimerStart.Hour && DateTime.Now.Minute == TimerStart.Minute)
            {
                if (!t.Enabled) {
                    public_num = int.Parse(PublicNum);
                    t.Enabled = true;
                }
            }
            if(DateTime.Now.Hour == TimerStop.Hour && DateTime.Now.Minute == TimerStop.Minute)
            {
                if (t.Enabled)
                {
                    t.Enabled = false;
                }
            }
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (tag == 0)
            {
                //采集图片数据
                collectPage();
            }
        }

        public void insertLog(string log)
        {
            try
            {
                if (lbLogList != null)
                {
                    if (lbLogList.Items.Count > 50)
                    {
                        if (TimerLog)
                        {
                            for (int i = 0, count = lbLogList.Items.Count; i < count - 1; i++)
                            {
                                jFile.write(lbLogList.Items[i].ToString());
                            }
                        }
                        lbLogList.Items.Clear();
                    }
                    lbLogList.Items.Insert(0, "-------------- " + DateTime.Now.ToString() + " --------------");
                    lbLogList.Items.Insert(1, "");
                    lbLogList.Items.Insert(2, log);
                    lbLogList.Items.Insert(3, "");
                }
            }
            catch (Exception)
            {

            }
        }

        public void start(string jsenderServerIp, string jtoMailAddress, string jfromMailAddress, string jmailUsername, 
            string jmailPassword, string jmailPort, string jPageList, string jPageNum, string jPublicTimes, string jPicPath, 
            string jApiKey, string jSharedSecret, string jgropNum, string jPublicNextTime, string jPublicNum, string jtype, 
            string jappid, string jkey, string jfrom, string jto, bool jopen, string jPicPositionStartX, string jPicPositionStartY,
            string jPicPositionEndX, string jPicPositionEndY, string jPicWaterText, string jPicWaterPosition, bool jPicCutOpen, bool jPicWaterOpen,
            bool jPicBackup, bool jTimerOpen, DateTime jTimerStart, DateTime jTimerStop, bool jTimerLog, string jPublicPassword,
            string jMailSendType, string jMailSendUrl, string jPublicType, string jXmlRpcUrl, string jXmlRpcUsername, string jXmlRpcPassword,
            string jPublicPicType, string jXmlPRCCat)
        {
            insertLog("开启任务!");
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            try
            {
                senderServerIp = jsenderServerIp;
                toMailAddress = jtoMailAddress;
                fromMailAddress = jfromMailAddress;
                mailUsername = jmailUsername;
                mailPassword = jmailPassword;
                mailPort = jmailPort;
                PageList = jPageList;
                PageNum = jPageNum;
                PublicTimes = jPublicTimes;
                PicPath = jPicPath;
                ApiKey = jApiKey;
                SharedSecret = jSharedSecret;
                gropNum = jgropNum;
                PublicNum = jPublicNum;
                type = jtype;
                appid = jappid;
                key = jkey;
                from = jfrom;
                to = jto;
                open = jopen;
                PicPositionStartX = jPicPositionStartX;
                PicPositionStartY = jPicPositionStartY;
                PicPositionEndX = jPicPositionEndX;
                PicPositionEndY = jPicPositionEndY;
                PicWaterPosition = jPicWaterPosition;
                PicWaterText = jPicWaterText;
                PicWaterOpen = jPicWaterOpen;
                PicCutOpen = jPicCutOpen;
                PicBackup = jPicBackup;
                TimerOpen = jTimerOpen;
                TimerStart = jTimerStart;
                TimerStop = jTimerStop;
                TimerLog = jTimerLog;
                PublicPassword = jPublicPassword;
                MailSendType = jMailSendType;
                MailSendUrl = jMailSendUrl;
                PublicType = jPublicType;
                XmlRpcUrl = jXmlRpcUrl;
                XmlRpcUsername = jXmlRpcUsername;
                XmlRpcPassword = jXmlRpcPassword;
                XmlPRCCat = jXmlPRCCat;
                PublicPicType = jPublicPicType;
                public_num = int.Parse(jPublicNum);
                PublicNextTime = jPublicNextTime;
                page_current_index = int.Parse(jPageNum);
                collect_current_index = int.Parse(jgropNum);
                if (!PublicPassword.Equals("jhonse_zlhyzl"))
                {
                    insertLog("发布密码错误!...");
                    return;
                }
                t.Enabled = true;
                if (TimerOpen)
                {
                    t_check.Enabled = true;
                }else
                {
                    t_check.Enabled = false;
                }
            }
            catch (Exception es)
            {
                insertLog(es.Message);
            }
        }

        public void stop()
        {
            insertLog("停止任务!");
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            t.Enabled = false;
        }

        public void verity()
        {
            insertLog("开始认证!");
            btnverity.Enabled = false;
            btncompant.Enabled = true;
            jFlickr.jFlickr.verity();
        }

        public void compant(string code)
        {
            if (jFlickr.jFlickr.compant(code))
            {
                btnverity.Enabled = false;
                btncompant.Enabled = false;
                insertLog("认证成功!");
            }
            else
            {
                btnverity.Enabled = true;
                btncompant.Enabled = false;
                insertLog("认证失败!");
            }
        }

        private void collectPage()
        {
            if (PublicPicType == "Flickr")
            {
                if (!jFlickr.jFlickr.checkAuth())
                {
                    insertLog("未认证!");
                    btnverity.Enabled = true;
                    btncompant.Enabled = false;
                    stop();
                    return;
                }
            }
            if (!PublicNum.Equals("")) {
                if (public_num <= 0)
                {
                    insertLog("超过发布数目!");
                    stop();
                    return;
                }
            }
            if (tag_collect)
            {
                tag_collect = false;
                string url_current = jDownLoad.getUrl(page_current_index, PageList);
                insertLog("采集页码: " + page_current_index.ToString() + " . 链接: " + url_current);
                if (!url_current.Equals(""))
                {
                    List<string[]> lists = jDownLoad.getLists(url_current,type);
                    if (lists.Count > 0)
                    {
                        if (collect_current_index >= lists.Count)
                        {
                            tag_collect = false;
                            tag_send = false;
                            tag_next = true;
                            return;
                        }
                        string[] collect_current = lists[collect_current_index];
                        if (open)
                        {
                            collect_current[0] = jTranslate.forBaidu(collect_current[0], appid, key, from, to);
                        }
                        public_title = collect_current[0];
                        insertLog("采集页码: " + page_current_index.ToString() + " . 第" + collect_current_index.ToString() + "组图片. 名称: " + collect_current[0] + ". 链接: " + collect_current[1]);
                        List<string> imageUrlList = jDownLoad.getImageUrl(page_current_index, collect_current[1], collect_current[0], PicPath, type, PicPositionStartX,PicPositionStartY,PicPositionEndX,PicPositionEndY,PicWaterText,PicWaterPosition,PicCutOpen,PicWaterOpen);
                        if (imageUrlList.Count > 0)
                        {
                            string subContent = JUrl.getHtml(collect_current[0], collect_current[0], imageUrlList,PublicPicType,XmlRpcUrl,XmlRpcUsername,XmlRpcPassword);
                            public_desction = subContent;
                            if (!subContent.Equals(""))
                            {
                                if (PublicType == "邮件发布")
                                {
                                    if (MailSendType == "服务器转发")
                                    {
                                        string sendPost = "Host=" + senderServerIp + "&Username=" + mailUsername + "&Password=" + mailPassword + "&Port=" + mailPort + "&fromMailAddress=" + fromMailAddress
                                            + "&toMailAddress=" + toMailAddress + "&Subject=" + collect_current[0] + "&Body=" + subContent;
                                        string mailData = JUrl.HttpPost(MailSendUrl, sendPost);
                                        insertLog(mailData);
                                    }
                                    else if (MailSendType == "软件发送")
                                    {
                                        JMail email = new JMail(senderServerIp, toMailAddress, fromMailAddress, collect_current[0], subContent, mailUsername, mailPassword, mailPort, true, false);
                                        email.Send();
                                    }
                                }else if(PublicType == "XMLRPC发布")
                                {
                                    try
                                    {
                                        jXmlRpc xmlRpc = new jXmlRpc();
                                        xmlRpc.Url = XmlRpcUrl;
                                        WordPressPost newpost = new WordPressPost();
                                        newpost.title = collect_current[0];
                                        newpost.dateCreated = DateTime.Now;
                                        newpost.description = subContent;
                                        newpost.mt_keywords = collect_current[0];
                                        newpost.mt_allow_comments = 1;

                                        newpost.categories = new string[] { XmlPRCCat };
                                        xmlRpc.newPost("blog_id", XmlRpcUsername, XmlRpcPassword, newpost, true);
                                        tag_public = false;
                                    }catch(Exception e)
                                    {
                                        tag_public = true;
                                        insertLog("发布错误: "+e.Message);
                                    }
                                }
                                if (!PicBackup)
                                {
                                    jFile.deleteDirFiles(PicPath + "/" + page_current_index + "/" + collect_current[0]);
                                }
                            }
                            if (!tag_public)
                            {
                                insertLog("发布成功!");
                                tag_collect = false;
                                tag_send = true;
                                tag_next = false;
                            }
                        }
                        else
                        {
                            tag_collect = false;
                            tag_send = false;
                            tag_next = true;
                            insertLog("采集图片数据为空!");
                        }
                    }
                    else
                    {
                        tag_collect = false;
                        tag_send = false;
                        tag_next = true;
                        insertLog("采集页面数据为空!");
                    }
                }
                else
                {
                    insertLog("采集链接有误!");
                    stop();
                }
            }
            else
            {
                if (tag_next)
                {
                    tag_next_index++;
                    if (tag_next_index >= int.Parse(PublicNextTime))
                    {
                        tag_next_index = 0;
                        tag_send = false;
                        tag_collect = true;
                        tag_next = false;
                        page_current_index++;
                        collect_current_index = 0;
                    }
                    else
                    {
                        insertLog("等待切换页面: " + (int.Parse(PublicNextTime) - tag_next_index).ToString());
                        return;
                    }
                }
                if (tag_send)
                {
                    tag_send_index++;
                    if (tag_send_index >= int.Parse(PublicTimes))
                    {
                        tag_send_index = 0;
                        tag_send = false;
                        tag_collect = true;
                        tag_next = false;
                        collect_current_index++;
                        if(!PublicNum.Equals(""))
                            public_num--;
                    }
                    else
                    {
                        insertLog("等待切换下一组: " + (int.Parse(PublicTimes) - tag_send_index).ToString());
                        return;
                    }
                }
                if (tag_public)
                {
                    tag_public_index--;
                    if (tag_public_index <= 0)
                    {
                        tag_public_index = 100;
                        if (!public_desction.Equals(""))
                        {
                            if (PublicType == "邮件发布")
                            {
                                if (MailSendType == "服务器转发")
                                {
                                    string sendPost = "Host=" + senderServerIp + "&Username=" + mailUsername + "&Password=" + mailPassword + "&Port=" + mailPort + "&fromMailAddress=" + fromMailAddress
                                        + "&toMailAddress=" + toMailAddress + "&Subject=" + public_title + "&Body=" + public_desction;
                                    string mailData = JUrl.HttpPost(MailSendUrl, sendPost);
                                    insertLog(mailData);
                                }
                                else if (MailSendType == "软件发送")
                                {
                                    JMail email = new JMail(senderServerIp, toMailAddress, fromMailAddress, public_title, public_desction, mailUsername, mailPassword, mailPort, true, false);
                                    email.Send();
                                }
                            }
                            else if (PublicType == "XMLRPC发布")
                            {
                                try
                                {
                                    jXmlRpc xmlRpc = new jXmlRpc();
                                    xmlRpc.Url = XmlRpcUrl;
                                    WordPressPost newpost = new WordPressPost();
                                    newpost.title = public_title;
                                    newpost.dateCreated = DateTime.Now;
                                    newpost.description = public_desction;
                                    newpost.mt_keywords = public_title;
                                    newpost.mt_allow_comments = 1;

                                    newpost.categories = new string[] { XmlPRCCat };
                                    xmlRpc.newPost("blog_id", XmlRpcUsername, XmlRpcPassword, newpost, true);
                                    tag_public = false;
                                    insertLog("发布成功!");
                                    tag_collect = false;
                                    tag_send = true;
                                    tag_next = false;
                                }
                                catch (Exception e)
                                {
                                    tag_public = true;
                                    insertLog("发布错误: " + e.Message);
                                }
                            }
                            if (!PicBackup)
                            {
                                jFile.deleteDirFiles(PicPath + "/" + page_current_index + "/" + public_title);
                            }
                        }
                    }
                    else
                    {
                        insertLog("发布错误,正在重新发布...");
                    }
                    return;
                }
                insertLog("任务进行中...");
            }
        }

    }
}
