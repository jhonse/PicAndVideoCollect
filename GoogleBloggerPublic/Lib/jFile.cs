using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib
{
    class jFile
    {
        public static bool is_Dir(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void deleteDirFiles(string path)
        {
            try
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            catch (Exception)
            {

            }
        }

        public static void deleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {

            }
        }

        public static void write(string log)
        {
            try
            {
                string Dir = Environment.CurrentDirectory;
                if (!Directory.Exists(Dir + "/" + DateTime.Now.Year + "-" + DateTime.Now.Month))
                {
                    Directory.CreateDirectory(Dir + "/" + DateTime.Now.Year + "-" + DateTime.Now.Month);
                }
                FileStream fs = new FileStream(Dir + "/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "/" + DateTime.Now.Day + ".txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.WriteLine(log);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {

            }
        }

        public static void writePublicMsg(string name, string msg) {
            try
            {
                string Dir = Environment.CurrentDirectory;
                if (!Directory.Exists(Dir + "/PublicMsg"))
                {
                    Directory.CreateDirectory(Dir + "/PublicMsg");
                }
                FileStream fs = new FileStream(Dir + "/PublicMsg/" + name + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine(msg);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {

            }
        }

        public static void writeUploadMsg(string name, string msg)
        {
            try
            {
                string Dir = Environment.CurrentDirectory;
                if (!Directory.Exists(Dir + "/UploadMsg"))
                {
                    Directory.CreateDirectory(Dir + "/UploadMsg");
                }
                FileStream fs = new FileStream(Dir + "/UploadMsg/" + name + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine(msg);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {

            }
        }

        public static string[] readPublicMsg()
        {
            string[] Msg = new string[3];
            try
            {
                string Dir = Environment.CurrentDirectory;
                DirectoryInfo folder = new DirectoryInfo(Dir + "/PublicMsg");
                foreach (FileInfo file in folder.GetFiles("*.txt"))
                {
                    Msg[0] = file.Name.Substring(0,file.Name.Length - 4);
                    Msg[1] = File.ReadAllText(file.FullName);
                    Msg[2] = file.FullName;
                    break;
                }
            }
            catch (Exception)
            {
                
            }
            return Msg;
        }

        public static string[] readUploadMsg()
        {
            string[] Msg = new string[3];
            try
            {
                string Dir = Environment.CurrentDirectory;
                DirectoryInfo folder = new DirectoryInfo(Dir + "/UploadMsg");
                foreach (FileInfo file in folder.GetFiles("*.txt"))
                {
                    Msg[0] = file.Name.Substring(0, file.Name.Length - 4);
                    Msg[1] = File.ReadAllText(file.FullName);
                    Msg[2] = file.FullName;
                    break;
                }
            }
            catch (Exception)
            {

            }
            return Msg;
        }

        public static int readUploadCount()
        {
            int count = 0;
            try
            {
                string Dir = Environment.CurrentDirectory;
                DirectoryInfo folder = new DirectoryInfo(Dir + "/UploadMsg");
                count = folder.GetFiles("*.txt").Length;
            }
            catch (Exception)
            {

            }
            return count;
        }

        public static void PubicMsgMove(string sPath,string dPath)
        {
            try
            {
                File.Move(sPath, dPath);
            }
            catch (Exception)
            {

            }
        }

        public static byte[] getBits(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static string getMimeType(string path)
        {
            string mimeType = "application/octet-stream";
            string ext = Path.GetExtension(path).ToLower();
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }
    }
}
