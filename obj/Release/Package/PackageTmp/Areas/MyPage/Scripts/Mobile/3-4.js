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
                var totalCount = result.TotalCount;

                var current_bxsliders = document.getElementsByClassName("bxslider");
                var bxslider_count = current_bxsliders.length;

                var lines = $("#group-list");

                for (var i = 0; i < group.length; i++) {

                    var c = group[i];

                    var line = '<p class="subtitle3"><a href="/mypage/group/' + c.GroupID + '/detail">' + c.GroupName + '</a></p>' + 
                        '<div class="mem-list">' +
                            '<ul class="bxslider appendbxslider-' + bxslider_count + '">';
                    for (j = 0; j < c.ProfileMember.length; j++)
                    {
                        var m = c.ProfileMember[j];
                        if (m.ProfileImage != null)
                        {
                            line = line +
                                '<li><a href="/user/' + m.MemberId + '"><img class="circle" src="' + m.ProfileImage + '" alt="' + m.Nickname + '" /></a></li>';
                        }
                    }
                    line = line + '</ul>' +
                    '</div>' +
                    '<p class="link-right fs8"><a class="leavegroup" id=' + c.GroupID.toString() + ' href="#">メンバーから抜ける</a></p>';

                    lines.append(line);

                }
                var newCount = group.length * 1 + groupcount;
                $('#GetMoreGroupButton').data("groupcount", newCount);

                if (totalCount <= newCount)
                    $('#GetMoreGroupButton').hide();

                // 追加したbxslider のreload
                var config = {
                    slideWidth: 35,
                    slideMargin: 5,
                    minSlides: 1,
                    maxSlides: 7,
                    moveSlides: 1,
                }
                var sliders = new Array();
                var appendbxslider_class = ".appendbxslider-" + bxslider_count;
                $(appendbxslider_class).each(function (i, slider) {
                    sliders[i] = $(slider).bxSlider();
                });
                $.each(sliders, function (i, slider) {
                    slider.reloadSlider(config)
                });

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("エラーが起こりました。" + errorThrown);
            }
        });
    }

    var groupID = $("#MgroupID").val();

    //Click グループを作成する
    $(".mytitle").click(function (event) {
        var urlname = "MyPage/group/" + groupID + "/edit";
        var linkUrl = '@Url.Action("Index", urlname, new { area = "MyPage", groupID })';
        $('#MyPageGroupEdit').load(linkUrl);
    });

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