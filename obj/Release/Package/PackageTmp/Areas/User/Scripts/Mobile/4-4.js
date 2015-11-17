var followingcount;

$(document).ready(function () {

    getMoreFollow(0);

    //フォローのもっとみる検索
    $("#GetMoreFollowingsButton").click(function (event) {

        followingcount = $(this).data("followingcount");

        getMoreFollow(followingcount);

    });

});


function getMoreFollow(followingcount) {
    var member_id = $("#currentUser").attr("data-currentmemberid");
    var other_member_id = $('#GetMoreFollowingsButton').data("other_member_id");
    var url = '/user/following/' + other_member_id + "/" + followingcount + '/';

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ followingcount: followingcount }),
        async: false,
        success: function (result) {

            var following = result.FollowingMembers;
            var topics = result.TopicTitles;
            var totalCount = result.TotalCount;

            var lines = $("#lines");

            for (var i = 0; i < following.length; i++) {

                var m = following[i];

                var formattedLastExpectedPointDate = "";
                if (m.FormattedLastExpectedPointDate != null) {
                    formattedLastExpectedPointDate = "最終予想 " + m.FormattedLastExpectedPointDate;
                }

                var displayFollow = "display:none";
                var displayUnfollow = "display:none";
                if (member_id > 0) {
                    var displayFollow = (m.IsFollowing || m.IsLoginUser) ? "display:none" : "";
                    var displayUnfollow = (m.IsFollowing && !m.IsLoginUser) ? "" : "display:none";
                }

                var nameLink = m.IsLoginUser ? "/mypage/" : "/user/" + m.MemberId + "/";

                var line = '<li>' +
                    '        <div class="m-col01">' +
                    '        <a href="/user/' + m.MemberId + '">' +
                    '        <img class="circle" src="' + m.ProfileImg + '" alt="' + m.Nickname + '">' +
                    '        </a> ' +
                    '        </div>' +
                    '        <div class="m-col02">' +
                    '        <h4>' +
                    '        <a href="/user/' + m.MemberId + '">' + m.Nickname + '</a> ' +
                    '        </h4>' +
                    '        <p class="fs8">' + m.FormattedDisplayPoints + ' pt ' + formattedLastExpectedPointDate + '</p>' +
                    '        </div>' +
                    '    <div class="m-col03">' +
                    '        <a class="unfollow_link" id="unfollow-' + m.MemberId + '" style="' + displayUnfollow + '">フォロー中</a>' +
                    '        <a class="my-btn01 follow_link" id="follow-' + m.MemberId + '" style="' + displayFollow + '">フォローする</a>' +
                    '    </div>' +
                    '</li>';


                lines.append(line);

                $("#follow-" + m.MemberId).click(function (event) { follow(event); });
                $("#unfollow-" + m.MemberId).click(function (event) { unfollow(event); });

            }

            var newCount = following.length * 1 + followingcount;
            $('#GetMoreFollowingsButton').data("followingcount", newCount);

            if (totalCount <= newCount)
                $('#GetMoreFollowingsButton').hide();


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("エラーが起こりました。" + errorThrown);
        }
    });
}