using FlickrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib.jFlickr
{
    class FlickrManager
    {
        public const string ApiKey = "1879c811b79c8cb322f3a756ee3dada4";
        public const string SharedSecret = "cab27126f984bcb5";

        public static Flickr GetInstance()
        {
            return new Flickr(ApiKey, SharedSecret);
        }

        public static Flickr GetAuthInstance()
        {
            var f = new Flickr(ApiKey, SharedSecret);
            f.OAuthAccessToken = OAuthToken.Token;
            f.OAuthAccessTokenSecret = OAuthToken.TokenSecret;
            return f;
        }

        public static OAuthAccessToken OAuthToken
        {
            get
            {
                return Properties.Settings.Default.OAuthToken;
            }
            set
            {
                Properties.Settings.Default.OAuthToken = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
