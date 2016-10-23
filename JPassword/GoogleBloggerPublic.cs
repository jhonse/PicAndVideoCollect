using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPassword
{
    public class GoogleBloggerPublic
    {
        public static bool getPublic(string password)
        {
            if (password.Equals("jhonse_zlhyzl"))
            {
                return true;
            }else
            {
                return false;
            }
        }

        public static bool getShow(string password)
        {
            if (password.Equals("jhonse"))
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}
