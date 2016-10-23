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
                //Directory.CreateDirectory(path);
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
