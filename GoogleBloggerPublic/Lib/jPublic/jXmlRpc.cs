using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib.jPublic
{
    /// <summary>
    /// 定义文章结构
    /// </summary>
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct WordPressPost
    {
        public DateTime dateCreated;
        /// <summary>
        /// 文章正文内容
        /// </summary>
        public string description;
        public string title;
        public string postid;
        /// <summary>
        /// 分组名称即可
        /// </summary>
        public string[] categories;
        /// <summary>
        /// 1是可以评论，0是邪恶的关平
        /// </summary>
        public int mt_allow_comments;
        public int mt_allow_pings;
        public int mt_convert_breaks;
        public string mt_text_more;
        /// <summary>
        /// 文章摘要
        /// </summary>
        public string mt_excerpt;
        /// <summary>
        /// Tags，也叫标签
        /// </summary>
        public string mt_keywords;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct WordPressUploadData
    {
        public string name;
        public string type;
        public byte[] bits;
        public bool overwrite;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct WordPressUploadDataResult
    {
        public string id;
        public string file;
        public string url;
        public string type;
    }

    class jXmlRpc : XmlRpcClientProtocol
    {
        [XmlRpcMethod("metaWeblog.newPost")]
        public string newPost(string blogid, string username,
        string password, WordPressPost content, bool publish)
        {
            return (string)this.Invoke
            ("newPost",
                new object[] { blogid, username, password, content, publish
            });
        }

        [XmlRpcMethod("metaWeblog.newMediaObject")]
        public WordPressUploadDataResult newMediaObject(string blogid, string username,
        string password, WordPressUploadData data)
        {

            return (WordPressUploadDataResult)this.Invoke
            ("newMediaObject",
                new object[] { blogid, username, password, data
            });
        }

    }
}
