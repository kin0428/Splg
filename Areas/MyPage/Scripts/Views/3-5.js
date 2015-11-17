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

    var url = '/mypage/following/' + followingcount + '/';

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

                var displayFollow = m.IsFollowing ? "display:none" : "";
                var displayUnfollow = m.IsFollowing ? "" : "display:none";

                var line = '<div class="sub_list_row">' +
                    '<h4>' +
                    '    <a href="/user/' + m.MemberId + '">' +
                    '    <img class="circle resimg" src="' + m.ProfileImg + '" alt="" />' +
                    '    <span class="fs14 blue">' + m.Nickname + '</span>' +
                    '    </a>' +
                    '    <span class="fs14"> ' + m.FormattedDisplayPoints + 'pt  </span>' +
                    '    <span class="fs10 gray2">' + formattedLastExpectedPointDate + '</span>' +
                    '</h4>' +
                    '<a class="unfollow_link" id="unfollow-' + m.MemberId + '" style="' + displayUnfollow + '">フォロー中</a>' +
                    '<a class="block_04_7_btn01 follow_link" id="follow-' + m.MemberId + '" style="' + displayFollow + '">フォローする</a>' +
                '</div>';

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