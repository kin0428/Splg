using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.MyPage
{
    public class MyPageConstant
    {
        public const string	TOP_INDEX = "3-1";	//マイページ
        public const string	EXPECTEDLIST_INDEX = "3-2";	//予想リスト
        public const string	ARTICLE_INDEX = "3-3";	//投稿記事
        public const string	GROUP_INDEX = "3-4";	//グループリスト
        public const string	GROUP_DETAILS = "3-4-1";	//グループページ
        public const string	GROUP_NEW = "3-4-2";	//グループ作成
        public const string	GROUP_EDIT = "3-4-3";	//グループ編集
        public const string	FOLLOWING_INDEX = "3-5";	//フォローリスト
        public const string	FOLLOWERS_INDEX = "3-6";	//フォロワーリスト
        public const string	SETTING_INDEX = "3-7";	//設定
        public const string SETTING_ADDRESS = "3-7-1";	//設定　メールアドレス変更
        public const string SETTING_PASSWORD = "3-7-2";	//設定　パスワード変更
        public const string SETTING_DELETEACCOUNT = "3-7-3";	//設定　アカウントの削除
        public const string	NOTICE_INDEX = "3-8";	//お知らせ
        public const string	USERSEARCH_INDEX = "3-9";	//ユーザー検索
        public const string RECENT_EXPECTEDLIST_INDEX = "3-10";	//予想結果

        // MyPageGroupListで利用
        public const int GROUPLIST_GROUP_MAX = 10;      //表示するグループ数max
        public const int GROUPLIST_MEMBERIMAGE_MAX = 8; //表示する会員イメージ数max
    
        // 既読フラグ
        public const bool ALREADY_READ = true;
    }
}