using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.User
{
    public class UserConstant
    {
        public const string TOP_INDEX = "4-1";	//他ユーザーページ
        public const string EXPECTEDLIST_INDEX = "4-2";	//他ユーザー　予想リスト
        public const string ARTICLE_INDEX = "4-3";	//他ユーザー　投稿記事
        public const string FOLLOWING_INDEX = "4-4";	//他ユーザー　フォローリスト
        public const string FOLLOWERS_INDEX = "4-5";	//他ユーザー　フォロワーリスト

        // MyPageGroupListで利用
        public const int GROUPLIST_GROUP_MAX = 10;      //表示するグループ数max
        public const int GROUPLIST_MEMBERIMAGE_MAX = 8; //表示する会員イメージ数max
    
    }
}