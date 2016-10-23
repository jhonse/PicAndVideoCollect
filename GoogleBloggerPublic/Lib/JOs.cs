using System;
using System.Windows.Forms;

namespace GoogleBloggerPublic.Lib
{
    class JOs
    {
        private static JThread jthread = null;

        public static void init(ListBox jlbLogList, Button jbtnverity, Button jbtncompant, Button jbtnStart, Button jbtnStop) {
            jthread = new JThread(jlbLogList,jbtnverity,jbtncompant,jbtnStart,jbtnStop);
            jthread.insertLog("初始化成功");
        }

        public static void start(string jsenderServerIp, string jtoMailAddress, string jfromMailAddress, string jmailUsername, 
            string jmailPassword, string jmailPort, string jPageList, string jPageNum, string jPublicTimes, string jPicPath, string jApiKey, 
            string jSharedSecret, string jgropNum, string jPublicNextTime, string jPublicNum, string jtype, string jappid, string jkey, string jfrom, 
            string jto, bool jopen,string jPicPositionStartX, string jPicPositionStartY, string jPicPositionEndX, string jPicPositionEndY, string jPicWaterText, 
            string jPicWaterPosition, bool jPicCutOpen, bool jPicWaterOpen, bool jPicBackup, bool jTimerOpen, DateTime jTimerStart, DateTime jTimerStop,
            bool jTimerLog, string jPublicPassword, string jMailSendType, string jMailSendUrl, string jPublicType, string jXmlRpcUrl, string jXmlRpcUsername, string jXmlRpcPassword,
            string jPublicPicType,string jXmlPRCCat)
        {
            jthread.start(jsenderServerIp, jtoMailAddress, jfromMailAddress, jmailUsername, jmailPassword, jmailPort, jPageList, jPageNum, jPublicTimes, 
                jPicPath, jApiKey, jSharedSecret, jgropNum, jPublicNextTime, jPublicNum, jtype,jappid,jkey,jfrom,jto,jopen,jPicPositionStartX,
                jPicPositionStartY,jPicPositionEndX,jPicPositionEndY,jPicWaterText,jPicWaterPosition,jPicCutOpen,jPicWaterOpen,
                jPicBackup,jTimerOpen,jTimerStart,jTimerStop, jTimerLog, jPublicPassword, jMailSendType, jMailSendUrl,jPublicType,jXmlRpcUrl,jXmlRpcUsername,
                jXmlRpcPassword, jPublicPicType, jXmlPRCCat);
        }

        public static void stop()
        {
            jthread.stop();
        }

        public static void verity() {
            jthread.verity();
        }

        public static void compant(string code) {
            jthread.compant(code);
        }

        public static void registerjHotKey(IntPtr hWnd)
        {
            //注册热键Shift+S，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
            jHotKey.RegisterHotKey(hWnd, 100, jHotKey.KeyModifiers.Ctrl, Keys.S);
            //注册热键Ctrl+B，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。
            jHotKey.RegisterHotKey(hWnd, 101, jHotKey.KeyModifiers.Ctrl, Keys.B);
            //注册热键Alt+D，Id号为102。HotKey.KeyModifiers.Alt也可以直接使用数字1来表示。
            jHotKey.RegisterHotKey(hWnd, 102, jHotKey.KeyModifiers.Ctrl, Keys.D);
        }

        public static void uregisterjHotKey(IntPtr hWnd)
        {
            //注销Id号为100的热键设定
            jHotKey.UnregisterHotKey(hWnd, 100);
            //注销Id号为101的热键设定
            jHotKey.UnregisterHotKey(hWnd, 101);
            //注销Id号为102的热键设定
            jHotKey.UnregisterHotKey(hWnd, 102);
        }
    }
}
