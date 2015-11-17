﻿var usersearchcount;
var usersearcoldkeyword;


$(document).ready(function () {

    getMembers(0);
    
    //検索する
    $("#submit").click(function (event) {

        usersearcoldkeyword = null;
        usersearchcount = 0;

        getMembers(usersearchcount);

    });

    //検索する
    $("#GetMoreUsersButton").click(function (event) {

        usersearchcount = $(this).data("usersearchcount");
        getMembers(usersearchcount);

    });


    function getMembers(usersearchcount) {
    
        var keyword = $("#iname").val();
        if (keyword.trim() == "")
            return;

        if (usersearcoldkeyword != keyword.trim()) {
            var lines = $("#lines");
            if (lines!= null)
                lines.empty();
            usersearchcount = 0;
        }

        usersearcoldkeyword = keyword.trim();

        var url = '/user_search/' + keyword + '/' + usersearchcount + '/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ keyword: keyword }),
            async: false,
            success: function (result) {

                var members = result.UserSearchMembers;
                $("#totalFoundCount")[0].innerText = result.TotalCount;

                var totalCount = result.TotalCount;

                var lines = $("#lines");

                for (var i = 0; i < members.length; i++) {

                    var m = members[i];

                    var formattedLastExpectedPointDate = '';
                    if (m.FormattedLastExpectedPointDate != null) {
                        formattedLastExpectedPointDate = "最終予想 " + m.FormattedLastExpectedPointDate;
                    }

                    var displayFollow = m.IsFollowing ? "display:none" : "";
                    var displayUnfollow = m.IsFollowing ? "" : "display:none";


                    var line = '<li>' +
                    '  <div class="m-col01"><img class="circle" src="' + m.ProfileImg + '" alt="" /></div>' +
                    '      <a href="/user/' + m.MemberId + '/"><div class="m-col02">' +
                    '      <h4>' + m.Nickname + '</h4></a>' +
                    '      <p class="fs8 follow_link">'  + m.FormattedDisplayPoints + ' pt 最終更新' + formattedLastExpectedPointDate + '</p>' +
                    '    </div>' +
                    '  <div class="m-col03"><a class="my-btn01 unfollow_link" id="unfollow-' + m.MemberId + '" style="' +  displayUnfollow +　'">フォロー中</a></div>' +
                    '  <div class="m-col03"><a class="my-btn01 follow_link" id="follow-' + m.MemberId + '" style="' +  displayFollow +　'">フォローする</a></div>' +
                    '</li>';

                    lines.append(line);

                    $("#follow-" + m.MemberId).click(function (event) { follow(event); });
                    $("#unfollow-" + m.MemberId).click(function (event) { unfollow(event); });

                }

                var newsearchcount = members.length * 1 + usersearchcount;
                $('#GetMoreUsersButton').data("usersearchcount", newsearchcount);

                if (totalCount <= newsearchcount)
                    $('#GetMoreUsersButton').fadeOut(0)
                else
                    $('#GetMoreUsersButton').fadeIn(0);


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("エラーが起こりました。" + errorThrown);
            }
        });
    }



});


