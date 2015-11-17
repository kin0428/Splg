var myPageGroupNewMembers = new Object();//追加等のために、ここにストアしておく

$(document).ready(function () {

    //getMoreFollowing(0);

    toastr.options = {
        "positionClass": "toast-top-center",
        "preventDuplicates": true
    }

    //もっとみる検索
    $("#GetMoreFollowingButton").click(function (event) {

        var followingcount = $(this).data("followingcount");

        getMoreFollowing(followingcount);

    });

    function getMoreFollowing(followingcount) {

        var url = '/mypage/MyPageGroupNew/GetMoreFollowing/';

        var search_string = $("#member_search_string").val();

        // 画面に表示されてるメンバーリストを作成する（追加の除外対象とする）
        // グループメンバー
        var group_member = document.getElementById("group_menber");
        var members = group_member.getElementsByClassName("sub_list_row");
        var member_id = [];
        for (var i = 0; i < members.length; i++) {
            member_id[i] = parseInt(members[i].id.split('-')[1]);
        }

        //フォローメンバー
        var follow_member = document.getElementById("follow_member");
        members = follow_member.getElementsByClassName("sub_list_row");
        var follow_member_id = [];
        for (var j = 0; j < members.length; j++) {
            follow_member_id[j] = parseInt(members[j].id.split('-')[1]);
        }

        $('#GetMoreFollowingButton').data("followingcount", members.length);

        var post_data = {
            MemberID: 0,
            GroupID: 0,
            GroupName: "",
            FollowerSearchString: search_string,
            GroupMemberIDList: member_id,
            FollowMemberIDList: follow_member_id
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

                var plists = $("#follow_member");
                var plist = "";
                for (i = 0; i < following.length; i++) {
                    var member_name = following[i].Nickname;
                    var member_image = following[i].ProfileImg;
                    var member_payofpoints = String(following[i].FormattedDisplayPoints).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
                    var member_expect = following[i].LastExpectedPointDate == null ? '' : '最終予想 ' + following[i].FormattedLastExpectedPointDate;
                    var member_id = following[i].MemberId;
                    plist = plist +
                        '<div class="sub_list_row" id="FollowMember-' + member_id + '">' +
                        '<h4>' +
                            '<img class="circle resimg" src="' + member_image + '" alt="" />' +
                            '<span class="fs14 blue"><a href="/user/' + member_id + '">' + member_name + ' </a></span>' +
                            '<span class="fs14"> ' + member_payofpoints + ' pt  </span>' +
                            '<span class="fs10 gray2">' + member_expect + '</span>' +
                        '</h4>' +
                        '<a class="block_04_7_btn01" id="' + member_id + '-' + member_image + '-' + member_name + '-' + member_payofpoints + '-' + member_expect + '" name="AddToGroup" href="#">グループに追加</a>' +
                    '</div>';
                }
                plists.append(plist);

                var followingcount = $("#GetMoreFollowingButton").data("followingcount");
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
    $("#member_search").click(function (event) {
        var search_string = $("#member_search_string").val();
        searchmember(search_string);
    });

    // グループから削除(Appenで追加されるので、ロード時バインドされていない）
    $(document).on("click", "[name='RemoveFromGroup']", function () {
        remove_member();
    });

    // グループに追加
    $(document).on("click", "[name='AddToGroup']", function () {
        add_member();
    });


    // グループ作成
    $("#make_grpoup").click(function (event) {
        var group_name = $("#group_name").val();
        if (!group_name) {
            alert("グループ名を入力して下さい。");
            return;
        }

        // グループメンバー
        var group_member = document.getElementById("group_menber");
        var members = group_member.getElementsByClassName("sub_list_row");
        var member_id = [];
        for (var i = 0; i < members.length; i++) {
            member_id[i] = parseInt(members[i].id.split('-')[1]);
        }

        //フォローメンバー
        var follow_member = document.getElementById("follow_member");
        members = follow_member.getElementsByClassName("sub_list_row");
        var follow_member_id = [];
        for (var j = 0; j < members.length; j++) {
            follow_member_id[j] = parseInt(members[j].id.split('-')[1]);
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
                    toastr.error(ErrorMessage);
                }
                else {
                    location.href = "/mypage/group/" + result.GroupID + "/edit/";
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                toastr.error("グループを作成できませんでした。");
            }
        });
    });

    function add_member() {
        var eventTarget = event.target || event.srcElement;
        var edit = eventTarget.id.split('-');
        var member_id = edit[0]
        var member_image = edit[1];
        var member_name = edit[2];
        var member_payofpoints = edit[3];
        var member_expect = edit[4] == null ? '' : '最終予想 ' + edit[4];
        var FollowMember = $("#FollowMember-" + member_id);
        FollowMember.remove();
        var plists = $("#group_menber");
        plist =
        '<div class="sub_list_row" id="GroupMember-' + member_id + '">' +
            '<h4>' +
                '<img class="circle resimg" src="' + member_image + '" alt="" />' +
                '<span class="fs14 blue"><a href="/user/' + member_id + '">' + member_name + ' </a></span>' +
                '<span class="fs14"> ' + member_payofpoints + ' pt  </span>' +
                '<span class="fs10 gray2">' + member_expect + '</span>' +
            '</h4>' +
            '<a class="block_04_7_btn01" id="' + member_id + '-' + member_image + '-' + member_name + '-' + member_payofpoints + '-' + member_expect + '" name="RemoveFromGroup" href="#">グループから外す</a>' +
        '</div>';
        plists.append(plist);
    }

    function remove_member() {
        var eventTarget = event.target || event.srcElement;
        var edit = eventTarget.id.split('-');
        var member_id = edit[0]
        var member_image = edit[1];
        var member_name = edit[2];
        var member_payofpoints = edit[3];
        var member_expect = edit[4] == null ? '' : '最終予想 ' + edit[4];
        var GroupMember = $("#GroupMember-" + member_id);
        GroupMember.remove();
        var plists = $("#follow_member");
        plist =
        '<div class="sub_list_row" id="FollowMember-' + member_id + '">' +
            '<h4>' +
                '<img class="circle resimg" src="' + member_image + '" alt="" />' +
                '<span class="fs14 blue"><a href="/user/' + member_id + '">' + member_name + ' </a></span>' +
                '<span class="fs14"> ' + member_payofpoints + ' pt  </span>' +
                '<span class="fs10 gray2">' + member_expect + '</span>' +
            '</h4>' +
            '<a class="block_04_7_btn01" id="' + member_id + '-' + member_image + '-' + member_name + '-' + member_payofpoints + '-' + member_expect + '" name="AddToGroup" href="#">グループに追加</a>' +
        '</div>';
        plists.append(plist);

    }

    function searchmember(search_string) {

        var url = '/mypage/MyPageGroupNew/SearchMember/';

        // グループメンバー
        var group_member = document.getElementById("group_menber");
        var members = group_member.getElementsByClassName("sub_list_row");
        var member_id = [];
        for (var i = 0; i < members.length; i++) {
            member_id[i] = parseInt(members[i].id.split('-')[1]);
        }

        //フォローメンバー
        var follow_member = $("#follow_member");
        if (follow_member != null)
            follow_member.empty();

        var post_data = {
            MemberID: 0,
            GroupID: 0,
            GroupName: "",
            FollowerSearchString: search_string,
            GroupMemberIDList: member_id,
            FollowMemberIDList: null
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
                $('#GetMoreFollowingButton').attr("data-followingcount", 0);

                var plists = $("#follow_member");
                var plist = "";
                for (var i = 0; i < following.length; i++) {
                    var member_name = following[i].Nickname;
                    var member_image = following[i].ProfileImg;
                    var member_payofpoints = String(following[i].FormattedDisplayPoints).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
                    var member_expect = following[i].FormattedLastExpectedPointDate == null ? '' : '最終予想 ' + following[i].FormattedLastExpectedPointDate;
                    var member_id = following[i].MemberId;
                    plist = plist +
                        '<div class="sub_list_row" id="FollowMember-' + member_id + '"' +
                        '<h4>' +
                            '<img class="circle resimg" src="' + member_image + '" alt="" />' +
                            '<span class="fs14 blue"><a href="/user/' + member_id + '">' + member_name + '</a></span>' +
                            '<span class="fs14"> ' + member_payofpoints + ' pt  </span>' +
                            '<span class="fs10 gray2">' + member_expect + '</span>' +
                        '</h4>' +
                        '<a class="block_04_7_btn01" id="' + member_id + '-' + member_image + '-' + member_name + '-' + member_payofpoints + '-' + member_expect + '" name="AddToGroup"  href="#">グループに追加</a>' +
                    '</div>';
                }
                plists.html(plist);

                var followingcount = $("#GetMoreFollowingButton").attr("data-followingcount");
                var newCount = following.length * 1 + followingcount * 1;
                $('#GetMoreFollowingButton').attr("data-followingcount", newCount);

                if (totalCount <= newCount)
                    $('#GetMoreFollowingButton').hide();
                else
                    $('#GetMoreFollowingButton').show();

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("エラーが起こりました。" + errorThrown);
            }
        });
    }

});