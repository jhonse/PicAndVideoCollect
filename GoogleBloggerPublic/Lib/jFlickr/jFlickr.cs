using FlickrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib.jFlickr
{
    class jFlickr
    {
        private static OAuthRequestToken requestToken;

        public static bool checkAuth()
        {
            if (FlickrManager.OAuthToken == null || FlickrManager.OAuthToken.Token == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string uploadPic(string path,string title,string description) {
            string photoid = "";
            try
            {
                var f = FlickrManager.GetAuthInstance();
                photoid = f.UploadPicture(path, title, description);
            }
            catch (Exception) {
                photoid = "";
            }
            return photoid;
        }

        public static string[] getPicUrl(string photoid) {
            string[] picUrl = new string[3];
            try
            {
                var f = FlickrManager.GetAuthInstance();
                PhotoInfo pi = f.PhotosGetInfo(photoid);
                if (pi != null) {
                    picUrl[0] = pi.SmallUrl;
                    picUrl[1] = pi.MediumUrl;
                    picUrl[2] = pi.LargeUrl;
                }
            }
            catch (Exception) {
                picUrl[0] = "";
                picUrl[1] = "";
                picUrl[2] = "";
            }
            return picUrl;
        }

        public static void verity() {
            Flickr f = FlickrManager.GetInstance();
            requestToken = f.OAuthGetRequestToken("oob");

            string url = f.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);

            System.Diagnostics.Process.Start(url);
        }

        public static bool compant(string code) {
            if (String.IsNullOrEmpty(code))
            {
                return false;
            }

            Flickr f = FlickrManager.GetInstance();
            try
            {
                var accessToken = f.OAuthGetAccessToken(requestToken, code);
                FlickrManager.OAuthToken = accessToken;
                return true;
            }
            catch (FlickrApiException)
            {
                return false;
            }
        }
    }
}
