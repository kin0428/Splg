$(document).ready(function () {

    getMoreGroup(0);

    //もっとみる検索
    $("#GetMoreGroupButton").click(function (event) {

        groupcount = $(this).data("groupcount");

        getMoreGroup(groupcount);

    });

    function getMoreGroup(groupcount) {

//        var url = '/mypage/group/' + groupcount + '/';
        var url = '/mypage/MyPageGroupList/GetMoreGroups/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ currentCount: groupcount }),
            async: false,
            success: function (result) {

                var group = result.GroupLists;
                var topics = result.TopicTitles;
                var totalCount = result.TotalCount;

                var lines = $("#group-list");

                for (var i = 0; i < group.length; i++) {

                    var c = group[i];

                    var line = '<li>' +
                        '<p ><a class="leavegroup" id="' + c.GroupID.toString() + '" href="#">メンバーから抜ける<</a></p>' +
                        '<dl>' +
                            '<dt><span class="blue"><a href="/mypage/group/' + c.GroupID+ '/detail">' + c.GroupName + '</a></span><br><span>' + c.NumberOfMember.toString() + '名</span></dt>' +
                            '<dd>';
                            for(j = 0; j < c.ProfileMember.length; j++)
                            {
                                if (c.ProfileMember[j].ProfileImage != null)
                                {
                                    line = line +
                                        '<a href="/user/' + c.ProfileMember[j].MemberId +
                                        '"><img class="resimg circle" src="' + c.ProfileMember[j].ProfileImage + '" alt="' + c.ProfileMember[j].Nickname + '" /></a>';
                                }
                            }
                            if (c.NumberOfOther > 0)
                            {
                                line = line +
                                    '<span class="count_num">' + '+' + c.NumberOfOther + '</span>';
                            }
                            line = line +
                            '</dd>' +
                        '</dl>' + 
                    '</li>';

                    lines.append(line);

                }

                var newCount = group.length * 1 + groupcount;
                $('#GetMoreGroupButton').data("groupcount", newCount);

                if (totalCount <= newCount)
                    $('#GetMoreGroupButton').hide();


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("エラーが起こりました。" + errorThrown);
            }
        });
    }
    var groupID = $("#MgroupID").val();
    ////Click グループを作成する
    //$(".mytitle").click(function (event) {
    //    var urlname = "MyPage/group/" + groupID + "/edit";
    //    var linkUrl = '@Url.Action("Index", urlname, new { area = "MyPage", groupID })';
    //    $('#MyPageGroupEdit').load(linkUrl);
    //    });

    // グループから抜ける
    $(".leavegroup").click(function (event) {
        var result = confirm("グループを抜けますか？")
            if (result == true) leaveGroup();
        });


    function leaveGroup() {
        var eventTarget = event.target || event.srcElement;
        var group_id;
        group_id = String(eventTarget.id);
        var url = '/mypage/MyPageGroupDetails/LeaveGroup/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ groupid: group_id }),
            async: false,
            success: function (result) {
                location.reload();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                location.reload();
            }
        });
    }

});