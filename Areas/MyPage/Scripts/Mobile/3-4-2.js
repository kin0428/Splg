$(document).ready(function () {

    //getMoreFollowing(0);

    //もっとみる検索
    $("#GetMoreFollowingButton").click(function (event) {

        followingcount = $(this).data("followingcount");

        getMoreFollowing(followingcount);

    });

    function getMoreFollowing(followingcount) {

        var url = '/mypage/MyPageGroupNew/GetMoreFollowing/';

        var search_string = $("#member_search").val();

        // 画面に表示されてるメンバーリストを作成する（追加の除外対象とする）
        var group_member = document.getElementById("group_menber");
        var members = group_member.getElementsByClassName("Group_Member");
        var member_id = [];
        for (var i = 0; i < members.length; i++) {
            member_id[i] = parseInt(members[i].id.split('-')[1]);
        }
        var j = member_id.length;

        var follow_member = document.getElementById("MemberList");
        members = follow_member.getElementsByClassName("following-member");
        for (i = 0; i < members.length; i++) {
            member_id[j++] = parseInt(members[i].id.split('-')[1]);
        }

        $('#GetMoreFollowingButton').data("followingcount", members.length);

        var post_data = {
            MemberID: 0,
            GroupID: 0,
            GroupName: "",
            FollowerSearchString: search_string,
            currentCountFollowing: followingcount,
            GroupMembers: null,
            FollowMembers: null,
            GroupMemberIDList: member_id
        };

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                var following = result.NewGroup.FollowMembers;
                var totalCount = result.TotalCount;

                var plists = $("#MemberList");
                var plist = "";
                for (i = 0; i < following.length; i++) {
                    var member_name = following[i].Nickname;
                    var member_image = following[i].ProfileImg;
                    var member_payofpoints = String(following[i].FormattedDisplayPoints).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
                    var member_expect = following[i].LastExpectedPointDate == null ? '' : '最終予想 ' + following[i].FormattedLastExpectedPointDate;
                    var member_id = following[i].MemberId;
                    plist = plist +
                    '<li class="following-member" id="sub_list_row-' + member_id + '">' +
                        '<div class="m-col01"><img class="circle" src="' + member_image + '" alt="" /></div>' +
                        '<div class="m-col02">' +
                            '<h4><a href="/user/' + member_id + '">' + member_name + '</a></h4>' +
                            '<p class="fs8">' + member_payofpoints + ' pt ' + member_expect + '</p>' +
                        '</div>' +
                        '<div class="m-col03"><a class="my-btn01" id="' + member_id + '" name="AddToGroup" href="#">追加</a></div>' +
                    '</li>';
                }
                plists.append(plist);

                var newCount = following.length * 1 + followingcount;
                $('#GetMoreFollowingButton').data("followingcount", newCount);

                if (totalCount <= newCount)
                    $('#GetMoreFollowingButton').hide();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("エラーが起こりました。" + errorThrown);
            }
        });
    }

    // フォローメンバー検索
    $(".search-btn").click(function (event) {
        var search_string = $("#member_search").val();
        searchmember(search_string);
    });

    // グループから削除(Appenで追加されるので、ロード時バインドされていない）
    $(document).on("click","#RemoveFromGroup", function() {
        remove_member();
    });

    // グループに追加
    $(document).on("click","[name='AddToGroup']", function() {
        add_member();
    });

    // グループ作成
    $("#SaveGroup").click(function (event) {
        var group_name = $("#group_name").val();
        if (!group_name) {
            alert("グループ名を入力して下さい。");
            return;
        }
        var group_member = document.getElementById("group_menber");
        var members = group_member.getElementsByClassName("Group_Member");
        var member_id = [];
        var edit;
        for (i = 0; i < members.length; i++) {
            edit = members[i].id;
            member_id[i] = parseInt(edit.split("-")[1]);
        }
        var post_data = {
            GroupName: group_name,
            MemberIDs: member_id
        };
        var url = '/mypage/MyPageGroupNew/AddGroup/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {
                if (result.ErrorMessage != null) {
                    alert(result.ErrorMessage);
                }
                else
                {
                    location.href = "/mypage/group/" + result.GroupID + "/edit/";
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    });

    function add_member() {

        var eventTarget = event.target || event.srcElement;
        var member_id = eventTarget.id;

        var post_data = {
            member_id: member_id
        };
        var url = '/mypage/MyPageGroupNew/GetMemberInfo/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {
                var member_id = result.MemberId;
                var member_name = result.Nickname;
                var member_image = result.ProfileImg;
                var member_payofpoints = result.FormattedDisplayPoints;
                var member_expect = result.LastExpectedPointDate == null ? '' : '最終予想 ' + result.LastExpectedPointDate;
                var sub_list_row = $("#sub_list_row-" + member_id);
                sub_list_row.remove();
                var plists = $("#group_menber");

                plist =
                plist = '<li class="Group_Member" id="Group_Member-' + member_id + '">' +
                    '<div class="m-col01"><img class="circle" src="' + member_image + '" alt="" /></div>' +
                    '<div class="m-col02">' +
                        '<h4><a href="/user/' + member_id + '">' + member_name + '</a></h4>' +
                        '<p class="fs8">' + member_payofpoints + ' pt ' + member_expect + '</p>' +
                    '</div>' +
                '<div class="m-col03"><a class="my-btn01" id="RemoveFromGroup" name="' + member_id + '" href="#">外す</a></div>' +
                '</li>';
                plists.append(plist);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });

    }

    function remove_member() {

        var eventTarget = event.target || event.srcElement;

        var member_id = eventTarget.name;

        var post_data = {
            member_id: member_id
        };
        var url = '/mypage/MyPageGroupNew/GetMemberInfo/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {
                var member_id = result.MemberId;
                var member_name = result.Nickname;
                var member_image = result.ProfileImg;
                var member_payofpoints = result.FormattedDisplayPoints;
                var member_expect = result.LastExpect;
                // グループから削除する。
                var sub_list_row = $("#Group_Member-" + member_id);
                sub_list_row.remove();
                // `フォローを再表示
                var search_string = $("#member_search").val();
                searchmember(search_string);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });

    }

    function searchmember(search_string) {

        var url = '/mypage/MyPageGroupNew/SearchMember/';

        var group_member = document.getElementById("group_menber");
        var members = group_member.getElementsByClassName("Group_Member");
        var member_id = [];
        var edit;
        for (i = 0; i < members.length; i++) {
            edit = members[i].id;
            member_id[i] = parseInt(edit.split("-")[1]);
        }
        var post_data = {
            MemberID: 0,
            GroupID: 0,
            GroupName: "",
            FollowerSearchString: search_string,
            currentCountFollowing: 0,
            GroupMembers: null,
            FollowMembers: null,
            GroupMemberIDList: member_id
        };

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                var following = result.NewGroup.FollowMembers;
                var totalCount = result.TotalCount;

                var plists = $("#MemberList");
                var plist = "";
                plist = plist + '<ul class="my-list">';
                for (i = 0; i < following.length; i++) {
                    var member_name = following[i].Nickname;
                    var member_image = following[i].ProfileImg;
                    var member_payofpoints = String(following[i].FormattedDisplayPoints).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
                    var member_expect = following[i].FormattedLastExpectedPointDate == null ? '' : '最終予想 ' + following[i].FormattedLastExpectedPointDate;
                    var member_id = following[i].MemberId;
                    plist = plist +
                    '<li class="following-member" id="sub_list_row-' + member_id + '">' +
                        '<div class="m-col01"><img class="circle" src="' + member_image + '" alt="" /></div>' +
                        '<div class="m-col02">' +
                            '<h4><a href="/user/' + member_id + '">' + member_name + '</a></h4>' +
                            '<p class="fs8">' + member_payofpoints + ' pt ' + member_expect + '</p>' +
                        '</div>' +
                        '<div class="m-col03"><a class="my-btn01" id="' + member_id + '" name="AddToGroup" href="#">追加</a></div>' +
                    '</li>';
                }
                plists.html(plist);

                var newCount = following.length * 1 + followingcount;
                $('#GetMoreFollowingButton').data("followingcount", newCount);

                if (totalCount <= newCount)
                    $('#GetMoreFollowingButton').hide();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("エラーが起こりました。" + errorThrown);
            }
        });
    }

});