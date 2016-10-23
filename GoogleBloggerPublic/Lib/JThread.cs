using GoogleBloggerPublic.Lib.jPublic;
using PicAndVideoCollect.Lib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Timers;

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
        private string PageEndNum = "";
        private string PublicTimes = "";
        private string PicPath = "";
        private string ApiKey = "";
        private string gropNum = "";
        private string PublicNextTime = "";
        private string PublicSendTime = "";
        private string PublicUploadTime = "";
        private string PublicChangeTime = "";
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
        private int tag_send_msg_index = 0;
        private int tag_upload_index = 0;

        private bool tag_collect = true;
        private bool tag_send = false;
        private bool tag_next = false;
        private bool tag_sendMsg = true;
        private bool tag_uploadMsg = true;
        private bool tag_sendMsg_node = false;
        private bool tag_uploadMsg_node = false;

        private ListBox lbLogList = null;
        private ListBox lbSendLogList = null;
        private ListBox lbPicLogList = null;
        private Button btnverity = null;
        private Button btncompant = null;
        private Button btnStart = null;
        private Button btnStop = null;
        private Button btnPublicStart = null;
        private Button btnPublicStop = null;
        private Button btnUploadStart = null;
        private Button btnUploadStop = null;
        private Button btnInit = null;

        System.Timers.Timer t = null;
        System.Timers.Timer t_send = null;
        System.Timers.Timer t_pic = null;
        System.Timers.Timer t_check = null;

        public JThread(ListBox jlbLogList, ListBox jlbSendLogList, ListBox jlbPicLogList, Button jbtnverity, Button jbtncompant, Button jbtnStart, Button jbtnStop, Button jbtnPublicStart, Button jbtnPublicStop, Button jbtnInit, Button jbtnUploadStart, Button jbtnUploadStop)
        {
            lbLogList = jlbLogList;
            lbSendLogList = jlbSendLogList;
            lbPicLogList = jlbPicLogList;
            btnverity = jbtnverity;
            btncompant = jbtncompant;
            btnStart = jbtnStart;
            btnStop = jbtnStop;
            btnPublicStart = jbtnPublicStart;
            btnPublicStop = jbtnPublicStop;
            btnUploadStart = jbtnUploadStart;
            btnUploadStop = jbtnUploadStop;
            btnInit = jbtnInit;

            t = new System.Timers.Timer(1000);
            t.Elapsed += T_Elapsed;
            t.AutoReset = true;
            t.Enabled = false;

            t_check = new System.Timers.Timer(1000);
            t_check.Elapsed += T_check_Elapsed;
            t_check.AutoReset = true;
            t_check.Enabled = false;

            t_send = new System.Timers.Timer(1000);
            t_send.Elapsed += T_send_Elapsed;
            t_send.AutoReset = true;
            t_send.Enabled = false;

            t_pic = new System.Timers.Timer(1000);
            t_pic.Elapsed += T_pic_Elapsed;
            t_pic.AutoReset = true;
            t_pic.Enabled = false;
        }

        private void T_pic_Elapsed(object sender, ElapsedEventArgs e)
        {
            sendPic();
        }

        private void T_send_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            sendMsg();
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
                                jFile.write("[采集] "+lbLogList.Items[i].ToString());
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

        public void insertSendLog(string log)
        {
            try
            {
                if (lbSendLogList != null)
                {
                    if (lbSendLogList.Items.Count > 20)
                    {
                        if (TimerLog)
                        {
                            for (int i = 0, count = lbSendLogList.Items.Count; i < count - 1; i++)
                            {
                                jFile.write("[发布] "+lbSendLogList.Items[i].ToString());
                            }
                        }
                        lbSendLogList.Items.Clear();
                    }
                    lbSendLogList.Items.Insert(0, "-------------- " + DateTime.Now.ToString() + " --------------");
                    lbSendLogList.Items.Insert(1, "");
                    lbSendLogList.Items.Insert(2, log);
                    lbSendLogList.Items.Insert(3, "");
                }
            }
            catch (Exception)
            {

            }
        }

        public void insertPicLog(string log)
        {
            try
            {
                if (lbPicLogList != null)
                {
                    if (lbPicLogList.Items.Count > 20)
                    {
                        if (TimerLog)
                        {
                            for (int i = 0, count = lbPicLogList.Items.Count; i < count - 1; i++)
                            {
                                jFile.write("[发布] " + lbPicLogList.Items[i].ToString());
                            }
                        }
                        lbPicLogList.Items.Clear();
                    }
                    lbPicLogList.Items.Insert(0, "-------------- " + DateTime.Now.ToString() + " --------------");
                    lbPicLogList.Items.Insert(1, "");
                    lbPicLogList.Items.Insert(2, log);
                    lbPicLogList.Items.Insert(3, "");
                }
            }
            catch (Exception)
            {

            }
        }

        public void first(string jsenderServerIp, string jtoMailAddress, string jfromMailAddress, string jmailUsername, 
            string jmailPassword, string jmailPort, string jPageList, string jPageNum, string jPublicTimes, string jPicPath, 
            string jApiKey, string jSharedSecret, string jgropNum, string jPublicNextTime, string jPublicNum, string jtype, 
            string jappid, string jkey, string jfrom, string jto, bool jopen, string jPicPositionStartX, string jPicPositionStartY,
            string jPicPositionEndX, string jPicPositionEndY, string jPicWaterText, string jPicWaterPosition, bool jPicCutOpen, bool jPicWaterOpen,
            bool jPicBackup, bool jTimerOpen, DateTime jTimerStart, DateTime jTimerStop, bool jTimerLog, string jPublicPassword,
            string jMailSendType, string jMailSendUrl, string jPublicType, string jXmlRpcUrl, string jXmlRpcUsername, string jXmlRpcPassword,
            string jPublicPicType, string jXmlPRCCat, string jPublicSendTime, string jPublicUploadTime, string jPageEndNum, string jPublicChangeTime)
        {
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
                PageEndNum = jPageEndNum;
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
                PublicSendTime = jPublicSendTime;
                PublicUploadTime = jPublicUploadTime;
                PublicChangeTime = jPublicChangeTime;
                page_current_index = int.Parse(jPageNum);
                collect_current_index = int.Parse(jgropNum);
                if (!PublicPassword.Equals("jhonse_zlhyzl"))
                {
                    insertLog("发布密码错误!...");
                    insertSendLog("发布密码错误!...");
                    return;
                }
                if (TimerOpen)
                {
                    t_check.Enabled = true;
                }else
                {
                    t_check.Enabled = false;
                }
                insertLog("数据更新成功!");
                insertSendLog("数据更新成功!");
                btnInit.Enabled = false;
                btnStart.Enabled = true;
                btnPublicStart.Enabled = true;
                btnUploadStart.Enabled = true;
            }
            catch (Exception es)
            {
                insertLog(es.Message);
            }
        }

        public void collectStart() {
            insertLog("开启采集任务!");
            t.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnInit.Enabled = false;
        }

        public void collectStop()
        {
            insertLog("停止采集任务!");
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            t.Enabled = false;
            if (!btnPublicStop.Enabled && !btnUploadStop.Enabled)
            {
                btnInit.Enabled = true;
            }
        }

        public void publicStart() {
            insertSendLog("开启发布任务!");
            t_send.Enabled = true;
            btnPublicStart.Enabled = false;
            btnPublicStop.Enabled = true;
            btnInit.Enabled = false;
        }

        public void publicStop() {
            insertSendLog("停止发布任务!");
            t_send.Enabled = false;
            btnPublicStart.Enabled = true;
            btnPublicStop.Enabled = false;
            if (!btnStop.Enabled && !btnUploadStop.Enabled)
            {
                btnInit.Enabled = true;
            }
        }

        public void uploadStart()
        {
            insertPicLog("开启上传任务!");
            t_pic.Enabled = true;
            btnUploadStart.Enabled = false;
            btnUploadStop.Enabled = true;
            btnInit.Enabled = false;
        }

        public void uploadStop()
        {
            insertPicLog("停止上传任务!");
            t_pic.Enabled = false;
            btnUploadStart.Enabled = true;
            btnUploadStop.Enabled = false;
            if (!btnStop.Enabled && !btnPublicStop.Enabled)
            {
                btnInit.Enabled = true;
            }
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
                    collectStop();
                    return;
                }
            }
            if (!PublicNum.Equals("")) {
                if (public_num <= 0)
                {
                    insertLog("超过采集数目!");
                    collectStop();
                    return;
                }
            }
            if(int.Parse(PageEndNum) > int.Parse(PageNum))
            {
                if(page_current_index > int.Parse(PageEndNum))
                {
                    insertLog("超过采集页码数!");
                    collectStop();
                    return;
                }
            }else
            {
                if (page_current_index < int.Parse(PageEndNum))
                {
                    insertLog("超过采集页码数!");
                    collectStop();
                    return;
                }
            }
            if(jFile.readUploadCount() >= int.Parse(PublicChangeTime))
            {
                insertLog("等待上传完才能采集...!");
                return;
            }
            if (tag_collect)
            {
                tag_collect = false;
                string url_current = jDownLoad.getUrl(page_current_index, PageList);
                insertLog("采集页码: " + page_current_index.ToString() + " . 链接: " + url_current);
                if (!url_current.Equals(""))
                {
                    List<string[]> lists = new List<string[]>();
                    while (true)
                    {
                        lists = jDownLoad.getLists(url_current, type);
                        if(lists.Count > 0)
                        {
                            break;
                        }
                    }
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
                        insertLog("采集页码: " + page_current_index.ToString() + " . 第" + collect_current_index.ToString() + "组图片. 名称: " + collect_current[0] + ". 链接: " + collect_current[1]);
                        List<string> imageUrlList = new List<string>();
                        while (true)
                        {
                            imageUrlList = jDownLoad.getImageUrl(page_current_index, collect_current[1], collect_current[0], PicPath, type, PicPositionStartX, PicPositionStartY, PicPositionEndX, PicPositionEndY, PicWaterText, PicWaterPosition, PicCutOpen, PicWaterOpen, PicBackup);
                            if(imageUrlList.Count > 0)
                            {
                                break;
                            }
                        }
                        if (imageUrlList.Count > 0)
                        {
                            string imageText = page_current_index+"\r\n";
                            foreach(string image in imageUrlList)
                            {
                                imageText += image + "\r\n";
                            }
                            jFile.writeUploadMsg(collect_current[0], imageText);
                            /*string subContent = JUrl.getHtml(this, PicBackup, collect_current[0], collect_current[0], imageUrlList,PublicPicType,XmlRpcUrl,XmlRpcUsername,XmlRpcPassword);
                            if (!subContent.Equals(""))
                            {
                                jFile.writePublicMsg(collect_current[0], subContent);
                                if (!PicBackup)
                                {
                                    jFile.deleteDirFiles(PicPath + "/" + page_current_index);
                                }
                            }*/
                            tag_collect = false;
                            tag_send = true;
                            tag_next = false;
                        }
                        else
                        {
                            tag_collect = false;
                            tag_send = false;
                            tag_next = true;
                            insertLog("采集图片数据为空!");
                        }
                        tag_collect = false;
                        tag_send = true;
                        tag_next = false;
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
                    collectStop();
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
                        if (int.Parse(PageEndNum) > int.Parse(PageNum)) {
                            page_current_index++;
                        }
                        else
                        {
                            page_current_index--;
                        }
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
            }
        }

        private void sendPic() {
            if (tag_uploadMsg)
            {
                tag_uploadMsg = false;
                string[] UploadMsgData = jFile.readUploadMsg();
                if (UploadMsgData[0] != null && UploadMsgData[1] != null && UploadMsgData[2] != null)
                {
                    try
                    {
                        string[] pics = UploadMsgData[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        List<string> imageUrlList = new List<string>();
                        string paget_current_index_index = pics[0];
                        foreach (string pic in pics)
                        {
                            if (jImage.is_pic(pic))
                            {
                                imageUrlList.Add(pic);
                            }
                        }
                        if (imageUrlList.Count > 0)
                        {
                            string subContent = JUrl.getHtml(this, PicBackup, UploadMsgData[0], UploadMsgData[0], imageUrlList, PublicPicType, XmlRpcUrl, XmlRpcUsername, XmlRpcPassword);
                            if (!subContent.Equals(""))
                            {
                                jFile.writePublicMsg(UploadMsgData[0], subContent);
                                if (!PicBackup)
                                {
                                    jFile.deleteFile(UploadMsgData[2]);
                                }
                                else
                                {
                                    jFile.PubicMsgMove(UploadMsgData[2], UploadMsgData[2] + ".bys");
                                }
                                if (!PicBackup)
                                {
                                    jFile.deleteDirFiles(PicPath + "/" + paget_current_index_index + "/" + UploadMsgData[0]);
                                }
                                tag_sendMsg_node = true;
                            }
                        }
                        else
                        {
                            insertPicLog("数据有误...");
                            tag_uploadMsg = true;
                        }
                    }
                    catch (Exception e)
                    {
                        insertPicLog(e.Message);
                        tag_uploadMsg = true;
                    }
                }
                else
                {
                    tag_uploadMsg = true;
                }
            }
            else
            {
                if (tag_uploadMsg_node)
                {
                    tag_upload_index++;
                    if (tag_upload_index >= int.Parse(PublicUploadTime))
                    {
                        tag_upload_index = 0;
                        tag_uploadMsg = true;
                        tag_uploadMsg_node = false;
                    }
                    else
                    {
                        insertPicLog("等待上传下一组: " + (int.Parse(PublicUploadTime) - tag_upload_index).ToString());
                        return;
                    }
                }
            }
        }

        private void sendMsg() {
            if (tag_sendMsg)
            {
                tag_sendMsg = false;
                string[] SendMsgData = jFile.readPublicMsg();
                if (SendMsgData[0] != null && SendMsgData[1] != null && SendMsgData[2] != null)
                {
                    if (PublicType == "邮件发布")
                    {
                        if (MailSendType == "服务器转发")
                        {
                            string sendPost = "Host=" + senderServerIp + "&Username=" + mailUsername + "&Password=" + mailPassword + "&Port=" + mailPort + "&fromMailAddress=" + fromMailAddress
                                + "&toMailAddress=" + toMailAddress + "&Subject=" + SendMsgData[0] + "&Body=" + SendMsgData[1];
                            string mailData = JUrl.HttpPost(MailSendUrl, sendPost);
                            insertLog(mailData);
                            tag_sendMsg_node = true;
                            jFile.PubicMsgMove(SendMsgData[2], SendMsgData[2] + ".bys");
                        }
                        else if (MailSendType == "软件发送")
                        {
                            JMail email = new JMail(senderServerIp, toMailAddress, fromMailAddress, SendMsgData[0], SendMsgData[1], mailUsername, mailPassword, mailPort, true, false);
                            email.Send();
                            tag_sendMsg_node = true;
                            jFile.PubicMsgMove(SendMsgData[2], SendMsgData[2] + ".bys");
                        }
                    }
                    else if (PublicType == "XMLRPC发布")
                    {
                        try
                        {
                            jXmlRpc xmlRpc = new jXmlRpc();
                            xmlRpc.Url = XmlRpcUrl;
                            WordPressPost newpost = new WordPressPost();
                            newpost.title = SendMsgData[0];
                            newpost.dateCreated = DateTime.Now;
                            newpost.description = SendMsgData[1];
                            newpost.mt_keywords = SendMsgData[0];
                            newpost.mt_allow_comments = 1;
                            newpost.categories = new string[] { XmlPRCCat };
                            string blog_id = xmlRpc.newPost("blog_id", XmlRpcUsername, XmlRpcPassword, newpost, true);
                            if(int.Parse(blog_id) > 0)
                            {
                                tag_sendMsg_node = true;
                                if (!PicBackup)
                                {
                                    jFile.deleteFile(SendMsgData[2]);
                                }else
                                {
                                    jFile.PubicMsgMove(SendMsgData[2], SendMsgData[2] + ".bys");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            insertSendLog(e.Message);
                            tag_sendMsg_node = true;
                        }

                    }
                }
                else
                {
                    tag_sendMsg = true;
                }
            }
            else
            {
                if (tag_sendMsg_node)
                {
                    tag_send_msg_index++;
                    if (tag_send_msg_index >= int.Parse(PublicSendTime))
                    {
                        tag_send_msg_index = 0;
                        tag_sendMsg = true;
                        tag_sendMsg_node = false;
                    }
                    else
                    {
                        insertSendLog("等待发布下一组: " + (int.Parse(PublicSendTime) - tag_send_msg_index).ToString());
                        return;
                    }
                }
            }
        }
    }
}
