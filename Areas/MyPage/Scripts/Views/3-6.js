var followercount;

$(document).ready(function () {

    getMoreFollowers(0);

    //フォロワーのもっとみる検索
    $("#GetMoreFollowersButton").click(function (event) {

        followercount = $(this).data("followercount");

        getMoreFollowers(followercount);

    });

});


function getMoreFollowers(followercount) {


    var url = '/mypage/followers/' + followercount + '/';

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ followercount: followercount }),
        async: false,
        success: function (result) {

            var followers = result.FollowerMembers;
            var topics = result.TopicTitles;
            var totalCount = result.TotalCount;

            var lines = $("#lines");

            for (var i = 0; i < followers.length; i++) {

                var m = followers[i];

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

            var newCount = followers.length * 1 + followercount;
            $('#GetMoreFollowersButton').data("followercount", newCount);

            if (totalCount <= newCount)
                $('#GetMoreFollowersButton').hide();


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("エラーが起こりました。" + errorThrown);
        }
    });

}